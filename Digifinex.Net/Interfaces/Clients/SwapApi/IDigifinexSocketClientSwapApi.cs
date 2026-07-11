using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Digifinex.Net.Objects.Models;
using Digifinex.Net.Objects.Models.Socket;

namespace Digifinex.Net.Interfaces.Clients.SwapApi
{
    /// <summary>
    /// Digifinex Swap Socket API streams (<c>wss://openapi.digifinex.com/swap_ws/v2/</c>).
    /// </summary>
    public interface IDigifinexSocketClientSwapApi : ISocketApiClient<DigifinexCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to public swap trade updates for one instrument.
        /// </summary>
        /// <param name="instrumentId">Instrument id, for example <c>BTCUSDTPERP</c></param>
        /// <param name="onMessage">Trade event handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string instrumentId, Action<DataEvent<DigifinexSwapTradeUpdateMessage>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public swap trade updates for one or more instruments.
        /// </summary>
        /// <param name="instrumentIds">Instrument ids, for example <c>BTCUSDTPERP</c></param>
        /// <param name="onMessage">Trade event handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string[] instrumentIds, Action<DataEvent<DigifinexSwapTradeUpdateMessage>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public ticker updates for every swap instrument.
        /// <para><a href="https://docs.digifinex.com/en-ww/swap/v2/websocket.html#all-ticker" /></para>
        /// </summary>
        /// <param name="onMessage">Ticker event handler.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<DigifinexSwapTicker[]>> onMessage, CancellationToken ct = default);
    }
}
