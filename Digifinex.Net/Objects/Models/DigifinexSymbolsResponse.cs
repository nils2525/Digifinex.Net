using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Envelope returned by Digifinex GET /v3/spot/symbols.
    /// </summary>
    public record DigifinexSymbolsResponse
    {
        /// <summary>
        /// ["<c>symbol_list</c>"] Per-symbol entries
        /// </summary>
        [JsonPropertyName("symbol_list")]
        public DigifinexSymbol[] SymbolList { get; set; } = Array.Empty<DigifinexSymbol>();

        /// <summary>
        /// ["<c>code</c>"] Response status code; <c>0</c> means success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
