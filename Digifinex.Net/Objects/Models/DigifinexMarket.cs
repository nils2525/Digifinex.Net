using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Market entry as returned by Digifinex GET /v3/markets. The endpoint returns a flat list of
    /// markets with precision and minimum-volume info, lower-case symbol names.
    /// </summary>
    public record DigifinexMarket
    {
        /// <summary>
        /// ["<c>market</c>"] Market name in <c>base_quote</c> form (for example <c>btc_usdt</c>)
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>volume_precision</c>"] Number of decimals supported for the order amount (base asset)
        /// </summary>
        [JsonPropertyName("volume_precision")]
        public int VolumePrecision { get; set; }

        /// <summary>
        /// ["<c>price_precision</c>"] Number of decimals supported for the order price (quote asset)
        /// </summary>
        [JsonPropertyName("price_precision")]
        public int PricePrecision { get; set; }

        /// <summary>
        /// ["<c>min_amount</c>"] Minimum order value expressed in the quote asset (notional)
        /// </summary>
        [JsonPropertyName("min_amount")]
        public decimal MinAmount { get; set; }

        /// <summary>
        /// ["<c>min_volume</c>"] Minimum order quantity expressed in the base asset
        /// </summary>
        [JsonPropertyName("min_volume")]
        public decimal MinVolume { get; set; }
    }
}
