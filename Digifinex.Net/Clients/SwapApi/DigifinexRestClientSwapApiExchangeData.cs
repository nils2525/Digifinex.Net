using CryptoExchange.Net.Objects;
using Digifinex.Net.Interfaces.Clients.SwapApi;
using Digifinex.Net.Objects.Models;

namespace Digifinex.Net.Clients.SwapApi
{
    /// <inheritdoc />
    internal class DigifinexRestClientSwapApiExchangeData : IDigifinexRestClientSwapApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly DigifinexRestClientSwapApi _baseClient;

        internal DigifinexRestClientSwapApiExchangeData(DigifinexRestClientSwapApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public Task<WebCallResult<DigifinexSwapInstrumentsResponse>> GetInstrumentsAsync(int? type = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("type", type);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/swap/v2/public/instruments", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexSwapInstrumentsResponse>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<WebCallResult<DigifinexSwapTradesResponse>> GetRecentTradesAsync(string instrumentId, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "instrument_id", instrumentId }
            };
            parameters.AddOptional("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/swap/v2/public/trades", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexSwapTradesResponse>(request, parameters, ct);
        }
    }
}
