using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Response payload returned by the Digifinex websocket server to a <c>server.ping</c> request:
    /// <c>{"error":null,"result":"pong","id":N}</c>. Distinct from <see cref="DigifinexSubscriptionResponse"/>
    /// because <c>result</c> is a literal string here, not the structured object subscribe responses use.
    /// </summary>
    internal record DigifinexPingResponse
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
        /// ["<c>result</c>"] Always <c>"pong"</c> on success
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }
    }
}
