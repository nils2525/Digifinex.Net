using Digifinex.Net.Interfaces.Clients.SpotApi;
using Digifinex.Net.Interfaces.Clients.SwapApi;
using CryptoExchange.Net.Interfaces.Clients;

namespace Digifinex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Digifinex REST API.
    /// </summary>
    public interface IDigifinexRestClient : IRestClient<DigifinexCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="IDigifinexRestClientSpotApi"/>
        IDigifinexRestClientSpotApi SpotApi { get; }

        /// <summary>
        /// Swap API endpoints
        /// </summary>
        /// <see cref="IDigifinexRestClientSwapApi"/>
        IDigifinexRestClientSwapApi SwapApi { get; }
    }
}
