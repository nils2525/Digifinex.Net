using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Envelope returned by Digifinex GET /v3/markets.
    /// </summary>
    public record DigifinexMarketsResponse
    {
        /// <summary>
        /// ["<c>data</c>"] Per-market entries
        /// </summary>
        [JsonPropertyName("data")]
        public DigifinexMarket[] Data { get; set; } = Array.Empty<DigifinexMarket>();

        /// <summary>
        /// ["<c>date</c>"] Server timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public long Date { get; set; }

        /// <summary>
        /// ["<c>code</c>"] Response status code; <c>0</c> means success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
