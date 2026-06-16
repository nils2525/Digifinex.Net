using CryptoExchange.Net.Interfaces.Clients;

namespace Digifinex.Net.Interfaces.Clients.SwapApi
{
    /// <summary>
    /// Digifinex Swap API endpoints.
    /// </summary>
    public interface IDigifinexRestClientSwapApi : IRestApiClient<DigifinexCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to retrieving swap market and system data.
        /// </summary>
        /// <see cref="IDigifinexRestClientSwapApiExchangeData" />
        IDigifinexRestClientSwapApiExchangeData ExchangeData { get; }
    }
}
