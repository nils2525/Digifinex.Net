using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Error envelope returned in the <c>error</c> field of a Digifinex websocket subscribe/unsubscribe
    /// response.
    /// </summary>
    public record DigifinexSocketError
    {
        /// <summary>
        /// ["<c>code</c>"] Numeric error code
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// ["<c>message</c>"] Human-readable error message
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
