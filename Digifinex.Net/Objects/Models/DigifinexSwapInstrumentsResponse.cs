using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Swap instruments response.
    /// </summary>
    public record DigifinexSwapInstrumentsResponse
    {
        /// <summary>
        /// ["<c>code</c>"] Response code, 0 on success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// ["<c>data</c>"] Instruments
        /// </summary>
        [JsonPropertyName("data")]
        public DigifinexSwapInstrument[] Instruments { get; set; } = Array.Empty<DigifinexSwapInstrument>();
    }
}
