using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Swap websocket request response.
    /// </summary>
    internal record DigifinexSwapSubscriptionResponse
    {
        /// <summary>
        /// ["<c>event</c>"] Event name
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>id</c>"] Echoed request id
        /// </summary>
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        /// <summary>
        /// ["<c>code</c>"] Response code, 1 on success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// ["<c>msg</c>"] Response message
        /// </summary>
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }
}
