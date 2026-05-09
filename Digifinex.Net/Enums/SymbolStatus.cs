using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Enums
{
    /// <summary>
    /// Trading status for a symbol as returned by the Digifinex <c>/v3/spot/symbols</c> endpoint.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// ["<c>TRADING</c>"] Symbol is open for trading
        /// </summary>
        [Map("TRADING")]
        Trading,
        /// <summary>
        /// ["<c>HALT</c>", "<c>HALTED</c>"] Trading on this symbol has been halted
        /// </summary>
        [Map("HALT", "HALTED")]
        Halted,
        /// <summary>
        /// ["<c>BREAK</c>"] Symbol is paused
        /// </summary>
        [Map("BREAK")]
        Break
    }
}
