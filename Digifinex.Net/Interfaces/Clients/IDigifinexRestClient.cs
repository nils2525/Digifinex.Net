using Digifinex.Net.Interfaces.Clients.SpotApi;
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
    }
}
