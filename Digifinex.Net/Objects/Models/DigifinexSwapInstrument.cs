using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Swap instrument information.
    /// </summary>
    public record DigifinexSwapInstrument
    {
        /// <summary>
        /// ["<c>instrument_id</c>"] Instrument id
        /// </summary>
        [JsonPropertyName("instrument_id")]
        public string InstrumentId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>type</c>"] Instrument type, for example <c>REAL</c>
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>contract_type</c>"] Contract type, for example <c>PERPETUAL</c>
        /// </summary>
        [JsonPropertyName("contract_type")]
        public string ContractType { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>base_currency</c>"] Base currency
        /// </summary>
        [JsonPropertyName("base_currency")]
        public string BaseCurrency { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>quote_currency</c>"] Quote currency
        /// </summary>
        [JsonPropertyName("quote_currency")]
        public string QuoteCurrency { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>clear_currency</c>"] Settlement currency
        /// </summary>
        [JsonPropertyName("clear_currency")]
        public string ClearCurrency { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>contract_value</c>"] Contract value
        /// </summary>
        [JsonPropertyName("contract_value")]
        public decimal ContractValue { get; set; }

        /// <summary>
        /// ["<c>contract_value_currency</c>"] Currency of the contract value
        /// </summary>
        [JsonPropertyName("contract_value_currency")]
        public string ContractValueCurrency { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>is_inverse</c>"] Whether this is an inverse contract
        /// </summary>
        [JsonPropertyName("is_inverse")]
        public bool IsInverse { get; set; }

        /// <summary>
        /// ["<c>is_trading</c>"] Whether trading is enabled
        /// </summary>
        [JsonPropertyName("is_trading")]
        public bool IsTrading { get; set; }

        /// <summary>
        /// ["<c>status</c>"] Contract status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>price_precision</c>"] Price precision
        /// </summary>
        [JsonPropertyName("price_precision")]
        public int PricePrecision { get; set; }

        /// <summary>
        /// ["<c>tick_size</c>"] Price tick size
        /// </summary>
        [JsonPropertyName("tick_size")]
        public decimal TickSize { get; set; }

        /// <summary>
        /// ["<c>min_order_amount</c>"] Minimum order amount in contracts
        /// </summary>
        [JsonPropertyName("min_order_amount")]
        public decimal MinOrderAmount { get; set; }

        /// <summary>
        /// ["<c>open_max_limits</c>"] Open position limits by leverage
        /// </summary>
        [JsonPropertyName("open_max_limits")]
        public DigifinexSwapOpenMaxLimit[] OpenMaxLimits { get; set; } = Array.Empty<DigifinexSwapOpenMaxLimit>();
    }
}
