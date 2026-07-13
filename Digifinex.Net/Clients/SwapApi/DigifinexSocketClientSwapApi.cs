using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net.WebSockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
using Digifinex.Net.Clients.MessageHandlers;
using Digifinex.Net.Interfaces.Clients.SwapApi;
using Digifinex.Net.Objects.Models;
using Digifinex.Net.Objects.Models.Socket;
using Digifinex.Net.Objects.Options;
using Digifinex.Net.Objects.Sockets;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Clients.SwapApi
{
    /// <inheritdoc cref="IDigifinexSocketClientSwapApi" />
    internal partial class DigifinexSocketClientSwapApi : SocketApiClient<DigifinexEnvironment, DigifinexAuthenticationProvider, DigifinexCredentials>, IDigifinexSocketClientSwapApi
    {
        /// <inheritdoc />
        public new DigifinexSocketOptions ClientOptions => (DigifinexSocketOptions)base.ClientOptions;

        protected override ErrorMapping ErrorMapping => DigifinexErrors.RestErrorMapping;

        private readonly ConcurrentDictionary<int, DateTime> _connectionStartTimes = new();
        private readonly Random _jitterRandom = new(Guid.NewGuid().GetHashCode());
        private readonly object _jitterLock = new();

        internal DigifinexSocketClientSwapApi(ILoggerFactory? loggerFactory, DigifinexSocketOptions options)
            : base(loggerFactory, DigifinexExchange.ExchangeName, options.Environment.SwapSocketBaseAddress, options, options.SwapOptions)
        {
            RateLimiter = DigifinexExchange.RateLimiter.Socket;
            MaxIndividualSubscriptionsPerConnection = 30;

            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(30),
                _ => new DigifinexSwapPingQuery(),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        _logger.LogWarning("[Sckt {SocketId}] Swap ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });

            RegisterPeriodicQuery(
                "LifetimeRecycle",
                TimeSpan.FromSeconds(60),
                connection =>
                {
                    var maxLifetime = ClientOptions.MaxConnectionLifetime;
                    if (maxLifetime <= TimeSpan.Zero)
                        return null!;

                    var start = _connectionStartTimes.GetOrAdd(connection.SocketId, _ =>
                    {
                        TimeSpan jitter;
                        lock (_jitterLock)
                            jitter = TimeSpan.FromSeconds(_jitterRandom.NextDouble() * 600);
                        return DateTime.UtcNow - jitter;
                    });

                    var age = DateTime.UtcNow - start;
                    if (age < maxLifetime)
                        return null!;

                    _logger.LogInformation(
                        "[Sckt {SocketId}] Proactively recycling swap connection (age {Age}, cap {Cap})",
                        connection.SocketId, age, maxLifetime);
                    _connectionStartTimes.TryRemove(connection.SocketId, out _);
                    if (connection is SocketConnection sc)
                        _ = sc.TriggerReconnectAsync();
                    return null!;
                },
                null);
        }

        protected override IMessageSerializer CreateSerializer()
            => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DigifinexExchange._serializerContext));

        protected override DigifinexAuthenticationProvider CreateAuthenticationProvider(DigifinexCredentials credentials)
            => new DigifinexAuthenticationProvider(credentials);

        protected override Task<Uri?> GetReconnectUriAsync(ISocketConnection connection)
        {
            _connectionStartTimes.TryRemove(connection.SocketId, out _);
            return base.GetReconnectUriAsync(connection);
        }

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType)
            => new DigifinexSwapSocketMessageHandler();

        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => DigifinexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public override ReadOnlySpan<byte> PreprocessStreamMessage(SocketConnection connection, WebSocketMessageType type, ReadOnlySpan<byte> data)
        {
            if (type != WebSocketMessageType.Binary)
                return data;

            return DecompressZLib(data);
        }

        private static ReadOnlySpan<byte> DecompressZLib(ReadOnlySpan<byte> input)
        {
            using var output = new MemoryStream();
            using var source = new MemoryStream(input.ToArray());
#if NET6_0_OR_GREATER
            using var decompressor = new ZLibStream(source, CompressionMode.Decompress);
#else
            source.Position = 2;
            using var decompressor = new DeflateStream(source, CompressionMode.Decompress);
#endif
            decompressor.CopyTo(output);
            return new ReadOnlySpan<byte>(output.GetBuffer(), 0, (int)output.Length);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string instrumentId, Action<DataEvent<DigifinexSwapTradeUpdateMessage>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new[] { instrumentId }, onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string[] instrumentIds, Action<DataEvent<DigifinexSwapTradeUpdateMessage>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, DigifinexSwapTradeUpdateMessage>((receiveTime, originalData, data) =>
            {
                var symbol = data.Data.FirstOrDefault()?.InstrumentId;
                onMessage(
                    new DataEvent<DigifinexSwapTradeUpdateMessage>(DigifinexExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(data.FullData ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Event)
                        .WithSymbol(symbol)
                    );
            });

            var subscription = new DigifinexSwapTradeSubscription(_logger, instrumentIds, internalHandler);
            return SubscribeAsync(subscription, ct);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<DigifinexSwapTicker[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, DigifinexSwapTickerUpdateMessage>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Length == 0 ? (DateTime?)null : data.Data.Max(static ticker => ticker.GetTimestamp());
                if (timestamp.HasValue && timestamp.Value != default)
                    UpdateTimeOffset(timestamp.Value);

                onMessage(
                    new DataEvent<DigifinexSwapTicker[]>(DigifinexExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Event)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new DigifinexSwapAllTickerSubscription(_logger, internalHandler);
            return SubscribeAsync(subscription, ct);
        }
    }
}
