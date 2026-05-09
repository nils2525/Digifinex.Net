using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models
{
    /// <summary>
    /// Server time as returned by Digifinex GET /v3/time. The value is a Unix timestamp in
    /// milliseconds; consumers convert via <see cref="DateTimeOffset.FromUnixTimeMilliseconds(long)"/>.
    /// </summary>
    public record DigifinexServerTime
    {
        /// <summary>
        /// ["<c>server_time</c>"] Server time as a Unix timestamp in milliseconds
        /// </summary>
        [JsonPropertyName("server_time")]
        public long ServerTime { get; set; }

        /// <summary>
        /// ["<c>code</c>"] Response status code; <c>0</c> means success
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
