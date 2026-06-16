using Digifinex.Net.Interfaces.Clients.SpotApi;
using Digifinex.Net.Interfaces.Clients.SwapApi;
using CryptoExchange.Net.Interfaces.Clients;

namespace Digifinex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Digifinex websocket API.
    /// </summary>
    public interface IDigifinexSocketClient : ISocketClient<DigifinexCredentials>
    {
        /// <summary>
        /// Spot Socket API streams (<c>wss://openapi.digifinex.com/ws/v1/</c>)
        /// </summary>
        /// <see cref="IDigifinexSocketClientSpotApi"/>
        IDigifinexSocketClientSpotApi SpotApi { get; }

        /// <summary>
        /// Swap API endpoints
        /// </summary>
        /// <see cref="IDigifinexSocketClientSwapApi"/>
        IDigifinexSocketClientSwapApi SwapApi { get; }
    }
}
