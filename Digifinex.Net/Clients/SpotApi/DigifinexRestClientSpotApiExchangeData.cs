using Digifinex.Net.Interfaces.Clients.SpotApi;
using Digifinex.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace Digifinex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class DigifinexRestClientSpotApiExchangeData : IDigifinexRestClientSpotApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly DigifinexRestClientSpotApi _baseClient;

        internal DigifinexRestClientSpotApiExchangeData(DigifinexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <summary>
        /// <a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />
        /// </summary>
        /// <inheritdoc />
        public Task<WebCallResult<DigifinexServerTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v3/time", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexServerTime>(request, null, ct);
        }

        #endregion

        #region Get Markets

        /// <summary>
        /// <a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />
        /// </summary>
        /// <inheritdoc />
        public Task<WebCallResult<DigifinexMarketsResponse>> GetMarketsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v3/markets", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexMarketsResponse>(request, null, ct);
        }

        #endregion

        #region Get Symbols

        /// <summary>
        /// <a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />
        /// </summary>
        /// <inheritdoc />
        public Task<WebCallResult<DigifinexSymbolsResponse>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v3/spot/symbols", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexSymbolsResponse>(request, null, ct);
        }

        #endregion

        #region Get Currencies

        /// <summary>
        /// <a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />
        /// </summary>
        /// <inheritdoc />
        public Task<WebCallResult<DigifinexCurrenciesResponse>> GetCurrenciesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v3/currencies", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexCurrenciesResponse>(request, null, ct);
        }

        #endregion

        #region Get Tickers

        /// <summary>
        /// <a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />
        /// </summary>
        /// <inheritdoc />
        public Task<WebCallResult<DigifinexTickerResponse>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v3/ticker", DigifinexExchange.RateLimiter.Rest, 1, false);
            return _baseClient.SendAsync<DigifinexTickerResponse>(request, parameters, ct);
        }

        #endregion
    }
}
