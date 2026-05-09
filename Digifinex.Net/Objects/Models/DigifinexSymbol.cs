using Digifinex.Net.Enums;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Symbol entry as returned by Digifinex GET /v3/spot/symbols. Contains richer per-symbol
    /// metadata than <see cref="DigifinexMarket"/> (status, base/quote split, allowed order types).
    /// </summary>
    public record DigifinexSymbol
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name in <c>BASE_QUOTE</c> uppercase form (for example <c>BTC_USDT</c>)
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>status</c>"] Trading status (for example <c>TRADING</c>)
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }

        /// <summary>
        /// ["<c>base_asset</c>"] Base asset symbol
        /// </summary>
        [JsonPropertyName("base_asset")]
        public string BaseAsset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>quote_asset</c>"] Quote asset symbol
        /// </summary>
        [JsonPropertyName("quote_asset")]
        public string QuoteAsset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>amount_precision</c>"] Number of decimals supported for the order amount (base asset)
        /// </summary>
        [JsonPropertyName("amount_precision")]
        public int AmountPrecision { get; set; }

        /// <summary>
        /// ["<c>price_precision</c>"] Number of decimals supported for the order price (quote asset)
        /// </summary>
        [JsonPropertyName("price_precision")]
        public int PricePrecision { get; set; }

        /// <summary>
        /// ["<c>minimum_amount</c>"] Minimum order quantity in the base asset
        /// </summary>
        [JsonPropertyName("minimum_amount")]
        public decimal MinimumAmount { get; set; }

        /// <summary>
        /// ["<c>minimum_value</c>"] Minimum order value (notional) in the quote asset
        /// </summary>
        [JsonPropertyName("minimum_value")]
        public decimal MinimumValue { get; set; }

        /// <summary>
        /// ["<c>zone</c>"] Trading zone identifier (for example <c>MainBoard</c>, <c>Innovation</c>)
        /// </summary>
        [JsonPropertyName("zone")]
        public string? Zone { get; set; }

        /// <summary>
        /// ["<c>order_types</c>"] Supported order types for this symbol
        /// </summary>
        [JsonPropertyName("order_types")]
        public string[] OrderTypes { get; set; } = Array.Empty<string>();
    }
}
