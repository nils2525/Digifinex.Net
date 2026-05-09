using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Envelope returned by Digifinex GET /v3/ticker. The exchange wraps the per-symbol entries
    /// in a <c>ticker</c> array plus a top-level <c>date</c> and <c>code</c>.
    /// </summary>
    public record DigifinexTickerResponse
    {
        /// <summary>
        /// ["<c>ticker</c>"] Per-symbol 24h ticker entries
        /// </summary>
        [JsonPropertyName("ticker")]
        public DigifinexTicker[] Ticker { get; set; } = Array.Empty<DigifinexTicker>();

        /// <summary>
        /// ["<c>date</c>"] Server timestamp; semantics match the global Digifinex envelope
        /// </summary>
        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// ["<c>code</c>"] Response status code; <c>0</c> means success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
