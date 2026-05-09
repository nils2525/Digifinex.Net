using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Successful subscribe/unsubscribe result body emitted in the <c>result</c> field of the
    /// JSON-RPC envelope. Shape: <c>{"status":"success"}</c>.
    /// </summary>
    public record DigifinexSocketResult
    {
        /// <summary>
        /// ["<c>status</c>"] Status string (<c>success</c> on a successful subscribe/unsubscribe)
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
