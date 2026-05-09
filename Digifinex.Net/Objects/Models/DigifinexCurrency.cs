using Digifinex.Net.Enums;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Currency/asset entry as returned by Digifinex GET /v3/currencies. The endpoint emits one
    /// row per (currency, chain) pair; merging the rows by currency yields the per-asset network
    /// list consumed by the wrapper-side asset client.
    /// </summary>
    public record DigifinexCurrency
    {
        /// <summary>
        /// ["<c>currency</c>"] Asset symbol (for example <c>BTC</c>)
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>chain</c>"] On-chain network name (for example <c>BTC</c>, <c>ERC20</c>)
        /// </summary>
        [JsonPropertyName("chain")]
        public string Chain { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>min_deposit_amount</c>"] Minimum deposit amount accepted on this network
        /// </summary>
        [JsonPropertyName("min_deposit_amount")]
        public decimal MinDepositAmount { get; set; }

        /// <summary>
        /// ["<c>min_withdraw_amount</c>"] Minimum withdrawal amount accepted on this network
        /// </summary>
        [JsonPropertyName("min_withdraw_amount")]
        public decimal MinWithdrawAmount { get; set; }

        /// <summary>
        /// ["<c>deposit_status</c>"] Whether deposits are currently enabled on this network
        /// </summary>
        [JsonPropertyName("deposit_status")]
        public AssetTransferStatus DepositStatus { get; set; }

        /// <summary>
        /// ["<c>withdraw_status</c>"] Whether withdrawals are currently enabled on this network
        /// </summary>
        [JsonPropertyName("withdraw_status")]
        public AssetTransferStatus WithdrawStatus { get; set; }

        /// <summary>
        /// ["<c>withdraw_fee_currency</c>"] Currency in which the withdraw fee is denominated
        /// </summary>
        [JsonPropertyName("withdraw_fee_currency")]
        public string? WithdrawFeeCurrency { get; set; }

        /// <summary>
        /// ["<c>min_withdraw_fee</c>"] Fixed minimum withdraw fee
        /// </summary>
        [JsonPropertyName("min_withdraw_fee")]
        public decimal MinWithdrawFee { get; set; }

        /// <summary>
        /// ["<c>withdraw_fee_rate</c>"] Withdraw fee rate (when applicable)
        /// </summary>
        [JsonPropertyName("withdraw_fee_rate")]
        public decimal WithdrawFeeRate { get; set; }
    }
}
