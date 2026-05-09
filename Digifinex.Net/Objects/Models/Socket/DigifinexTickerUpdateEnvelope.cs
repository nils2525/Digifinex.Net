using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Top-level envelope received for <c>ticker.update</c> / <c>all_ticker.update</c> push events.
    /// Carries the routing field plus the strongly-typed <see cref="DigifinexTickerUpdateMessage"/>
    /// body containing one or more tickers.
    /// </summary>
    public record DigifinexTickerUpdateEnvelope
    {
        /// <summary>
        /// ["<c>method</c>"] Either <c>ticker.update</c> (per-symbol) or <c>all_ticker.update</c>
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>params</c>"] Strongly-typed body carrying the ticker entries
        /// </summary>
        [JsonPropertyName("params")]
        public DigifinexTickerUpdateMessage Params { get; set; } = new DigifinexTickerUpdateMessage();
    }
}
