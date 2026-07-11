using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Swap ticker data returned by the REST ticker endpoints and websocket ticker channels.
    /// </summary>
    public record DigifinexSwapTicker
    {
        /// <summary>[<c>instrument_id</c>] Instrument id.</summary>
        [JsonPropertyName("instrument_id")]
        public string InstrumentId { get; set; } = string.Empty;

        /// <summary>[<c>index_price</c>] Index price. REST ticker endpoints only.</summary>
        [JsonPropertyName("index_price")]
        public decimal? IndexPrice { get; set; }

        /// <summary>[<c>mark_price</c>] Mark price. REST ticker endpoints only.</summary>
        [JsonPropertyName("mark_price")]
        public decimal? MarkPrice { get; set; }

        /// <summary>[<c>max_buy_price</c>] Maximum permitted buy price. REST ticker endpoints only.</summary>
        [JsonPropertyName("max_buy_price")]
        public decimal? MaxBuyPrice { get; set; }

        /// <summary>[<c>min_sell_price</c>] Minimum permitted sell price. REST ticker endpoints only.</summary>
        [JsonPropertyName("min_sell_price")]
        public decimal? MinSellPrice { get; set; }

        /// <summary>[<c>best_bid</c>] Best bid price.</summary>
        [JsonPropertyName("best_bid")]
        public decimal? BestBidPrice { get; set; }

        /// <summary>[<c>best_bid_size</c>] Best bid quantity.</summary>
        [JsonPropertyName("best_bid_size")]
        public decimal? BestBidQuantity { get; set; }

        /// <summary>[<c>best_ask</c>] Best ask price.</summary>
        [JsonPropertyName("best_ask")]
        public decimal? BestAskPrice { get; set; }

        /// <summary>[<c>best_ask_size</c>] Best ask quantity.</summary>
        [JsonPropertyName("best_ask_size")]
        public decimal? BestAskQuantity { get; set; }

        /// <summary>[<c>high_24h</c>] Highest trade price in the past 24 hours.</summary>
        [JsonPropertyName("high_24h")]
        public decimal? HighPrice24h { get; set; }

        /// <summary>[<c>open_24h</c>] Opening trade price for the past 24 hours.</summary>
        [JsonPropertyName("open_24h")]
        public decimal? OpenPrice24h { get; set; }

        /// <summary>[<c>low_24h</c>] Lowest trade price in the past 24 hours.</summary>
        [JsonPropertyName("low_24h")]
        public decimal? LowPrice24h { get; set; }

        /// <summary>[<c>last</c>] Last trade price.</summary>
        [JsonPropertyName("last")]
        public decimal? LastPrice { get; set; }

        /// <summary>[<c>last_qty</c>] Last trade quantity.</summary>
        [JsonPropertyName("last_qty")]
        public decimal? LastQuantity { get; set; }

        /// <summary>[<c>volume_24h</c>] Total trade volume in the past 24 hours.</summary>
        [JsonPropertyName("volume_24h")]
        public decimal? Volume24h { get; set; }

        /// <summary>[<c>volume_token_24h</c>] Total token volume in the past 24 hours. Websocket ticker channels only.</summary>
        [JsonPropertyName("volume_token_24h")]
        public decimal? TokenVolume24h { get; set; }

        /// <summary>[<c>price_change_percent</c>] Price change percentage. REST ticker endpoints only.</summary>
        [JsonPropertyName("price_change_percent")]
        public decimal? PriceChangePercent { get; set; }

        /// <summary>[<c>open_interest</c>] Open-interest value as supplied by the exchange.</summary>
        [JsonPropertyName("open_interest")]
        public string? OpenInterest { get; set; }

        /// <summary>[<c>timestamp</c>] Update timestamp in Unix milliseconds.</summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>Converts <see cref="Timestamp"/> to UTC.</summary>
        /// <returns>The update timestamp, or <see cref="DateTime.MinValue"/> when absent.</returns>
        public DateTime GetTimestamp()
            => Timestamp == 0 ? default : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).UtcDateTime;
    }
}
