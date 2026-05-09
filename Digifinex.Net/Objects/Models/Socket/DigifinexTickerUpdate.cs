using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Single ticker entry as carried inside the Digifinex <c>ticker.update</c> /
    /// <c>all_ticker.update</c> websocket message. Field names match the wire format.
    /// </summary>
    public record DigifinexTickerUpdate
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name (for example <c>BTC_USDT</c>)
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>open_24h</c>"] Open price for the rolling 24h window
        /// </summary>
        [JsonPropertyName("open_24h")]
        public decimal? Open24h { get; set; }

        /// <summary>
        /// ["<c>high_24h</c>"] High price for the rolling 24h window
        /// </summary>
        [JsonPropertyName("high_24h")]
        public decimal? High24h { get; set; }

        /// <summary>
        /// ["<c>low_24h</c>"] Low price for the rolling 24h window
        /// </summary>
        [JsonPropertyName("low_24h")]
        public decimal? Low24h { get; set; }

        /// <summary>
        /// ["<c>base_volume_24h</c>"] Volume in the base asset over the rolling 24h window
        /// </summary>
        [JsonPropertyName("base_volume_24h")]
        public decimal? BaseVolume24h { get; set; }

        /// <summary>
        /// ["<c>quote_volume_24h</c>"] Volume in the quote asset over the rolling 24h window
        /// </summary>
        [JsonPropertyName("quote_volume_24h")]
        public decimal? QuoteVolume24h { get; set; }

        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal? Last { get; set; }

        /// <summary>
        /// ["<c>last_qty</c>"] Last trade quantity
        /// </summary>
        [JsonPropertyName("last_qty")]
        public decimal? LastQuantity { get; set; }

        /// <summary>
        /// ["<c>best_bid</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("best_bid")]
        public decimal? BestBid { get; set; }

        /// <summary>
        /// ["<c>best_bid_size</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("best_bid_size")]
        public decimal? BestBidSize { get; set; }

        /// <summary>
        /// ["<c>best_ask</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("best_ask")]
        public decimal? BestAsk { get; set; }

        /// <summary>
        /// ["<c>best_ask_size</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("best_ask_size")]
        public decimal? BestAskSize { get; set; }

        /// <summary>
        /// ["<c>timestamp</c>"] Update timestamp in milliseconds since unix epoch
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
