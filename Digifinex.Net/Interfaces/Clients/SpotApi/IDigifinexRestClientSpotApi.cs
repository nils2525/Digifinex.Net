using CryptoExchange.Net.Interfaces.Clients;

namespace Digifinex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Digifinex Spot API endpoints
    /// </summary>
    public interface IDigifinexRestClientSpotApi : IRestApiClient<DigifinexCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IDigifinexRestClientSpotApiExchangeData" />
        IDigifinexRestClientSpotApiExchangeData ExchangeData { get; }
    }
}
