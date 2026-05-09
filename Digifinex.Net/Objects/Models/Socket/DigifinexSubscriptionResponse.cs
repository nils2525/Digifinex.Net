using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Response payload returned by the Digifinex websocket server to a subscribe/unsubscribe
    /// (and any other JSON-RPC) request. The server echoes the client-supplied <c>id</c>; either
    /// <c>error</c> or <c>result</c> is populated.
    /// </summary>
    internal record DigifinexSubscriptionResponse
    {
        /// <summary>
        /// ["<c>id</c>"] Echo of the client-supplied request id
        /// </summary>
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        /// <summary>
        /// ["<c>error</c>"] Error envelope; absent (or <c>null</c>) on success
        /// </summary>
        [JsonPropertyName("error")]
        public DigifinexSocketError? Error { get; set; }

        /// <summary>
        /// ["<c>result</c>"] Result body; present on success
        /// </summary>
        [JsonPropertyName("result")]
        public DigifinexSocketResult? Result { get; set; }
    }
}
