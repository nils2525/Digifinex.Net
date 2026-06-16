using System.Text.Json.Serialization;
using Digifinex.Net.Objects.Models;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Swap trade update message.
    /// </summary>
    public record DigifinexSwapTradeUpdateMessage
    {
        /// <summary>
        /// ["<c>event</c>"] Always <c>trades.update</c>
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>data</c>"] Trades
        /// </summary>
        [JsonPropertyName("data")]
        public DigifinexSwapTrade[] Data { get; set; } = Array.Empty<DigifinexSwapTrade>();
    }
}
