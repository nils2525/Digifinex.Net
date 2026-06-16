using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Swap trade.
    /// </summary>
    public record DigifinexSwapTrade
    {
        /// <summary>
        /// ["<c>instrument_id</c>"] Instrument id
        /// </summary>
        [JsonPropertyName("instrument_id")]
        public string InstrumentId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>trade_id</c>"] Trade id
        /// </summary>
        [JsonPropertyName("trade_id")]
        public string TradeId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>direction</c>"] Trade direction: 1 open long, 2 open short, 3 close long, 4 close short
        /// </summary>
        [JsonPropertyName("direction")]
        public string Direction { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>volume</c>"] Native contract quantity
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// ["<c>trade_time</c>"] Trade time in Unix milliseconds
        /// </summary>
        [JsonPropertyName("trade_time")]
        public long TradeTime { get; set; }

        /// <summary>
        /// Convert trade time to UTC <see cref="DateTime"/>.
        /// </summary>
        public DateTime GetTimestamp()
            => DateTimeOffset.FromUnixTimeMilliseconds(TradeTime).UtcDateTime;
    }
}
