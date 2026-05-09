using Digifinex.Net.Converters;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Trade update message received from the Digifinex <c>trades.subscribe</c> websocket channel.
    /// Wire shape: <c>{"method":"trades.update","params":[clean, [trades], "BTC_USDT"]}</c>.
    /// The custom <see cref="DigifinexTradeUpdateConverter"/> projects that heterogeneous params
    /// array onto the strongly-typed properties below.
    /// </summary>
    [JsonConverter(typeof(DigifinexTradeUpdateConverter))]
    public record DigifinexTradeUpdate
    {
        /// <summary>
        /// First params entry: <c>true</c> when the update represents a complete (snapshot) state
        /// rather than a delta.
        /// </summary>
        public bool Clean { get; set; }

        /// <summary>
        /// Second params entry: the trade entries themselves.
        /// </summary>
        public DigifinexTradeEntry[] Trades { get; set; } = Array.Empty<DigifinexTradeEntry>();

        /// <summary>
        /// Third params entry: the symbol (for example <c>BTC_USDT</c>).
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
    }
}
