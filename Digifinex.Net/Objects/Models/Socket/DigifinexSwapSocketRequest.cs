using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Swap websocket request payload.
    /// </summary>
    internal record DigifinexSwapSocketRequest
    {
        /// <summary>
        /// ["<c>event</c>"] Event name
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>id</c>"] Client-supplied request id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// ["<c>instrument_id</c>"] Single instrument id
        /// </summary>
        [JsonPropertyName("instrument_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InstrumentId { get; set; }

        /// <summary>
        /// ["<c>instrument_ids</c>"] Instrument ids
        /// </summary>
        [JsonPropertyName("instrument_ids")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? InstrumentIds { get; set; }
    }
}
