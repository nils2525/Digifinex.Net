using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Swap trades response.
    /// </summary>
    public record DigifinexSwapTradesResponse
    {
        /// <summary>
        /// ["<c>code</c>"] Response code, 0 on success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// ["<c>data</c>"] Trades
        /// </summary>
        [JsonPropertyName("data")]
        public DigifinexSwapTrade[] Trades { get; set; } = Array.Empty<DigifinexSwapTrade>();
    }
}
