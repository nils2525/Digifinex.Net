using CryptoExchange.Net.Objects.Errors;

namespace Digifinex.Net
{
    /// <summary>
    /// Digifinex error code mappings.
    /// <see href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />
    /// </summary>
    internal static class DigifinexErrors
    {
        // The catalog is intentionally minimal: only entries the wrapper integration tests have
        // exercised should be added. Wrapper-side mapping (<see cref="ExchangeErrorMapper"/>) is
        // the primary place for live-API-confirmed mappings.
        internal static ErrorMapping RestErrorMapping { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.RateLimitRequest, false, "Rate limit exceeded", "200005"),
            ]
        );
    }
}
