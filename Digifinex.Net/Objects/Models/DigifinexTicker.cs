using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// 24h ticker entry as returned by Digifinex GET /v3/ticker. Field names match the wire format.
    /// </summary>
    public record DigifinexTicker
    {
        /// <summary>
        /// ["<c>symbol</c>"] Market name (for example <c>btc_usdt</c>)
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal Last { get; set; }

        /// <summary>
        /// ["<c>high</c>"] Highest trade price in the rolling 24h window
        /// </summary>
        [JsonPropertyName("high")]
        public decimal High { get; set; }

        /// <summary>
        /// ["<c>low</c>"] Lowest trade price in the rolling 24h window
        /// </summary>
        [JsonPropertyName("low")]
        public decimal Low { get; set; }

        /// <summary>
        /// ["<c>buy</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("buy")]
        public decimal Buy { get; set; }

        /// <summary>
        /// ["<c>sell</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("sell")]
        public decimal Sell { get; set; }

        /// <summary>
        /// ["<c>vol</c>"] Volume in the base asset over the rolling 24h window
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Volume { get; set; }

        /// <summary>
        /// ["<c>base_vol</c>"] Volume in the quote asset over the rolling 24h window
        /// </summary>
        [JsonPropertyName("base_vol")]
        public decimal BaseVolume { get; set; }

        /// <summary>
        /// ["<c>change</c>"] 24h price change percentage (already expressed in percent)
        /// </summary>
        [JsonPropertyName("change")]
        public decimal Change { get; set; }
    }
}
