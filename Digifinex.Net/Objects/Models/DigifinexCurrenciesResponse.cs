using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Envelope returned by Digifinex GET /v3/currencies.
    /// </summary>
    public record DigifinexCurrenciesResponse
    {
        /// <summary>
        /// ["<c>data</c>"] Per-(currency, network) entries; merge by <c>currency</c> for the per-asset view
        /// </summary>
        [JsonPropertyName("data")]
        public DigifinexCurrency[] Data { get; set; } = Array.Empty<DigifinexCurrency>();

        /// <summary>
        /// ["<c>code</c>"] Response status code; <c>0</c> means success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
