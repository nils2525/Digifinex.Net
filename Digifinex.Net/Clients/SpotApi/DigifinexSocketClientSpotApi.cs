using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net.WebSockets;
using Digifinex.Net.Clients.MessageHandlers;
using Digifinex.Net.Interfaces.Clients.SpotApi;
using Digifinex.Net.Objects.Models.Socket;
using Digifinex.Net.Objects.Options;
using Digifinex.Net.Objects.Sockets;
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
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IDigifinexSocketClientSpotApi" />
    internal partial class DigifinexSocketClientSpotApi : SocketApiClient<DigifinexEnvironment, DigifinexAuthenticationProvider, DigifinexCredentials>, IDigifinexSocketClientSpotApi
    {
        #region fields
        /// <inheritdoc />
        public new DigifinexSocketOptions ClientOptions => (DigifinexSocketOptions)base.ClientOptions;

        protected override ErrorMapping ErrorMapping => DigifinexErrors.RestErrorMapping;

        // Tracks the effective start time of each live socket (UTC, already jittered) so the
        // periodic LifetimeRecycle task can recycle a connection before Digifinex's undocumented
        // ~2h server-side close kicks in.
        private readonly ConcurrentDictionary<int, DateTime> _connectionStartTimes = new();
        private readonly Random _jitterRandom = new(Guid.NewGuid().GetHashCode());
        private readonly object _jitterLock = new();
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of DigifinexSocketClientSpotApi
        /// </summary>
        internal DigifinexSocketClientSpotApi(ILoggerFactory? loggerFactory, DigifinexSocketOptions options)
            : base(loggerFactory, DigifinexExchange.ExchangeName, options.Environment.SocketBaseAddress, options, options.SpotOptions)
        {
            RateLimiter = DigifinexExchange.RateLimiter.Socket;

            // Server drops the connection after 60s of silence (verified live: dropped at exactly
            // 60s when no traffic is sent). Send an application-level ping every 30s to keep the
            // connection alive; reconnect on ping timeout.
            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(30),
                _ => new DigifinexPingQuery(),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });

            // Digifinex also closes connections at the ~2h mark independent of activity
            // (undocumented). Recycle proactively just under that cap so we
            // control the rotation - one connection at a time, instead of all of them dropping
            // together when the server decides to close them. Per-connection jitter staggers
            // recycles across a 10-minute window so connections opened together don't all hit
            // the threshold at once.
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
                        "[Sckt {SocketId}] Proactively recycling connection (age {Age}, cap {Cap})",
                        connection.SocketId, age, maxLifetime);
                    _connectionStartTimes.TryRemove(connection.SocketId, out _);
                    if (connection is SocketConnection sc)
                        _ = sc.TriggerReconnectAsync();
                    return null!;
                },
                null);
        }
        #endregion

        #region Methods
        protected override IMessageSerializer CreateSerializer()
            => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DigifinexExchange._serializerContext));

        protected override DigifinexAuthenticationProvider CreateAuthenticationProvider(DigifinexCredentials credentials)
            => new DigifinexAuthenticationProvider(credentials);

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType)
            => new DigifinexSocketMessageHandler();

        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => DigifinexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public override ReadOnlySpan<byte> PreprocessStreamMessage(SocketConnection connection, WebSocketMessageType type, ReadOnlySpan<byte> data)
        {
            // Digifinex docs say "use zlib deflate" and the wire actually carries zlib-wrapped
            // frames (CMF/FLG header + Adler-32 trailer per RFC 1950), not raw deflate. Text frames
            // (JSON-RPC subscribe acks, error envelopes) come through uncompressed.
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
            // Skip the 2-byte zlib header so DeflateStream sees raw deflate. The trailing Adler-32
            // checksum is ignored; this matches the framework's existing Decompress() in spirit.
            source.Position = 2;
            using var decompressor = new DeflateStream(source, CompressionMode.Decompress);
#endif
            decompressor.CopyTo(output);
            return new ReadOnlySpan<byte>(output.GetBuffer(), 0, (int)output.Length);
        }
        #endregion

        #region Subscriptions
        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DigifinexTradeUpdateMessage>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new[] { symbol }, onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string[] symbols, Action<DataEvent<DigifinexTradeUpdateMessage>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, DigifinexTradeUpdateMessage>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<DigifinexTradeUpdateMessage>(DigifinexExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Method)
                        .WithSymbol(data.Params.Symbol)
                    );
            });

            var subscription = new DigifinexTradeSubscription(_logger, symbols, internalHandler);
            return SubscribeAsync(subscription, ct);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string[] symbols, Action<DataEvent<DigifinexTickerUpdateEnvelope>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, DigifinexTickerUpdateEnvelope>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<DigifinexTickerUpdateEnvelope>(DigifinexExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Method)
                    );
            });

            var subscription = new DigifinexTickerSubscription(_logger, symbols, internalHandler);
            return SubscribeAsync(subscription, ct);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<DigifinexTickerUpdateEnvelope>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, DigifinexTickerUpdateEnvelope>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<DigifinexTickerUpdateEnvelope>(DigifinexExchange.ExchangeName, data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Method)
                    );
            });

            var subscription = new DigifinexAllTickerSubscription(_logger, internalHandler);
            return SubscribeAsync(subscription, ct);
        }
        #endregion
    }
}
