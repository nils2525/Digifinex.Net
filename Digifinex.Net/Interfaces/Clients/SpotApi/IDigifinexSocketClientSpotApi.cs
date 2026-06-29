using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace Digifinex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Digifinex Spot websocket subscriptions
    /// </summary>
    public interface IDigifinexSocketClientSpotApi : ISocketApiClient<DigifinexCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to public trade updates for a single symbol. Pushes are delivered via the
        /// <c>trades.update</c> channel.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v1/websocket.html" /></para>
        /// </summary>
        /// <param name="symbol">Symbol in <c>BASE_QUOTE</c> form (for example <c>BTC_USDT</c>)</param>
        /// <param name="onMessage">Handler invoked on every push</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DigifinexTradeUpdateMessage>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public trade updates for the supplied symbols. A single
        /// <c>trades.subscribe</c> call carries the entire <paramref name="symbols"/> array; the
        /// server emits one <c>trades.update</c> push per symbol and routing keeps the handler
        /// invocation per-symbol.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v1/websocket.html" /></para>
        /// </summary>
        /// <param name="symbols">Symbols in <c>BASE_QUOTE</c> form (for example <c>BTC_USDT</c>)</param>
        /// <param name="onMessage">Handler invoked on every push</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string[] symbols, Action<DataEvent<DigifinexTradeUpdateMessage>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to per-symbol ticker updates for the supplied symbols. Pushes are delivered
        /// via the <c>ticker.update</c> channel.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v1/websocket.html" /></para>
        /// </summary>
        /// <param name="symbols">Symbols in <c>BASE_QUOTE</c> form (for example <c>BTC_USDT</c>)</param>
        /// <param name="onMessage">Handler invoked on every push</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string[] symbols, Action<DataEvent<DigifinexTickerUpdateEnvelope>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates for every currently listed market via the
        /// <c>all_ticker.subscribe</c> channel. One subscribe covers all symbols; pushes arrive as
        /// <c>all_ticker.update</c> envelopes throttled to ~1Hz server-side, each carrying a
        /// partial slice of active tickers (full symbol coverage accumulates across pushes).
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v1/websocket.html" /></para>
        /// </summary>
        /// <param name="onMessage">Handler invoked on every push</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<DigifinexTickerUpdateEnvelope>> onMessage, CancellationToken ct = default);
    }
}
