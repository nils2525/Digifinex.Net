using Digifinex.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace Digifinex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Digifinex Spot exchange-data REST endpoints (server time, markets, symbols, currencies,
    /// tickers).
    /// </summary>
    public interface IDigifinexRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get the server time. Useful as a connectivity check.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexServerTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the list of trading markets and their precision/notional rules.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexMarketsResponse>> GetMarketsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the list of spot symbols including their trading status, base/quote, and supported
        /// order types. Provides a richer view than <see cref="GetMarketsAsync"/>.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexSymbolsResponse>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the list of currencies and their per-network deposit/withdraw status.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexCurrenciesResponse>> GetCurrenciesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the 24h tickers. When <paramref name="symbol"/> is supplied the response contains
        /// only that symbol's entry.
        /// <para><a href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" /></para>
        /// </summary>
        /// <param name="symbol">Optional market name (lowercase, for example <c>btc_usdt</c>)</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexTickerResponse>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);
    }
}
