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
        public Task<HttpResult<DigifinexSwapInstrumentsResponse>> GetInstrumentsAsync(int? type = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DigifinexExchange._parameterSerializationSettings);
            parameters.Add("type", type);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/swap/v2/public/instruments", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexSwapInstrumentsResponse>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<HttpResult<DigifinexSwapTradesResponse>> GetRecentTradesAsync(string instrumentId, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DigifinexExchange._parameterSerializationSettings)
            {
                { "instrument_id", instrumentId }
            };
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/swap/v2/public/trades", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexSwapTradesResponse>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<HttpResult<DigifinexSwapTickerResponse>> GetTickerAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Parameters(DigifinexExchange._parameterSerializationSettings)
            {
                { "instrument_id", instrumentId }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/swap/v2/public/ticker", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexSwapTickerResponse>(request, parameters, ct);
        }
    }
}
