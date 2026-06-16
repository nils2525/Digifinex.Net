using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Swap max-open-position limit tier.
    /// </summary>
    public record DigifinexSwapOpenMaxLimit
    {
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }

        /// <summary>
        /// ["<c>maint_margin_ratio</c>"] Maintenance margin ratio
        /// </summary>
        [JsonPropertyName("maint_margin_ratio")]
        public decimal? MaintenanceMarginRatio { get; set; }

        /// <summary>
        /// ["<c>max_limit</c>"] Maximum limit
        /// </summary>
        [JsonPropertyName("max_limit")]
        public decimal MaxLimit { get; set; }
    }
}
