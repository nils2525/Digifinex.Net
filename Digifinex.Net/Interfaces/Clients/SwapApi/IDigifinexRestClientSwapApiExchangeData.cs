using CryptoExchange.Net.Objects;
using Digifinex.Net.Objects.Models;

namespace Digifinex.Net.Interfaces.Clients.SwapApi
{
    /// <summary>
    /// Digifinex Swap exchange-data REST endpoints.
    /// </summary>
    public interface IDigifinexRestClientSwapApiExchangeData
    {
        /// <summary>
        /// Get swap instruments.
        /// <para><a href="https://docs.digifinex.com/en-ww/swap/v2/rest.html#instruments" /></para>
        /// </summary>
        /// <param name="type">Optional instrument type; 1 simulated, 2 real</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexSwapInstrumentsResponse>> GetInstrumentsAsync(int? type = null, CancellationToken ct = default);

        /// <summary>
        /// Get recent public trades for a swap instrument.
        /// <para><a href="https://docs.digifinex.com/en-ww/swap/v2/rest.html#recenttrades" /></para>
        /// </summary>
        /// <param name="instrumentId">Instrument id, for example <c>BTCUSDTPERP</c></param>
        /// <param name="limit">Result limit, 1-100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DigifinexSwapTradesResponse>> GetRecentTradesAsync(string instrumentId, int? limit = null, CancellationToken ct = default);
    }
}
