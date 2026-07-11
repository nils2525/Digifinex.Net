using Digifinex.Net.Objects.Models;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Swap all-ticker websocket update.
    /// </summary>
    public record DigifinexSwapTickerUpdateMessage
    {
        /// <summary>[<c>event</c>] Event name, normally <c>all_ticker.update</c>.</summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;

        /// <summary>[<c>data</c>] Ticker entries included in this update.</summary>
        [JsonPropertyName("data")]
        public DigifinexSwapTicker[] Data { get; set; } = [];
    }
}
