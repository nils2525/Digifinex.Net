using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Top-level envelope received for <c>trades.update</c> push events. Carries the routing
    /// fields plus the strongly-typed <see cref="DigifinexTradeUpdate"/> body.
    /// </summary>
    public record DigifinexTradeUpdateMessage
    {
        /// <summary>
        /// ["<c>method</c>"] Always <c>trades.update</c>
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>params</c>"] Strongly-typed body carrying the trade list and symbol
        /// </summary>
        [JsonPropertyName("params")]
        public DigifinexTradeUpdate Params { get; set; } = new DigifinexTradeUpdate();
    }
}
