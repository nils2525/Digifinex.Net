namespace Digifinex.Net.Enums
{
    /// <summary>
    /// Asset deposit/withdraw status as returned by Digifinex <c>/v3/currencies</c>.
    /// Numeric values match the wire format directly: <c>1</c> = enabled, <c>0</c> = disabled.
    /// </summary>
    public enum AssetTransferStatus
    {
        /// <summary>
        /// ["<c>0</c>"] Disabled
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// ["<c>1</c>"] Enabled
        /// </summary>
        Enabled = 1
    }
}
