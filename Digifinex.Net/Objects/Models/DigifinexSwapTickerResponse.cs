using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Response returned by the single swap ticker endpoint.
    /// </summary>
    public record DigifinexSwapTickerResponse
    {
        /// <summary>[<c>code</c>] Response code, 0 on success.</summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>[<c>data</c>] Ticker data.</summary>
        [JsonPropertyName("data")]
        public DigifinexSwapTicker Data { get; set; } = new();
    }
}
