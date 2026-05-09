using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// JSON-RPC style request payload sent to the Digifinex websocket APIs. Examples:
    /// <code>{"method":"trades.subscribe","id":42,"params":["BTC_USDT"]}</code>
    /// </summary>
    internal record DigifinexSocketRequest
    {
        /// <summary>
        /// ["<c>method</c>"] Method name (for example <c>trades.subscribe</c>, <c>ticker.subscribe</c>)
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>id</c>"] Client-supplied request id; the server echoes it back in the matching response
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// ["<c>params</c>"] Method parameters - typically the list of symbols to subscribe to.
        /// May be <c>null</c> for parameter-less methods such as <c>all_ticker.subscribe</c>.
        /// </summary>
        [JsonPropertyName("params")]
        public string[]? Params { get; set; }
    }
}
