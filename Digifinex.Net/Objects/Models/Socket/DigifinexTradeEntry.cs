using Digifinex.Net.Enums;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Single trade entry as carried inside a Digifinex <c>trades.update</c> message.
    /// </summary>
    public record DigifinexTradeEntry
    {
        /// <summary>
        /// ["<c>id</c>"] Trade id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// ["<c>time</c>"] Trade time as a Unix timestamp in seconds with optional fractional seconds
        /// (for example <c>1523339279.761838</c>). Use <see cref="GetTimestamp"/> to convert to
        /// a UTC <see cref="DateTime"/> while preserving microsecond precision.
        /// </summary>
        [JsonPropertyName("time")]
        public double Time { get; set; }

        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// ["<c>amount</c>"] Trade quantity in the base asset
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// ["<c>type</c>"] Aggressor side (taker side) - <c>buy</c> or <c>sell</c>
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Type { get; set; }

        /// <summary>
        /// Converts <see cref="Time"/> (fractional Unix seconds) to a UTC <see cref="DateTime"/>
        /// with microsecond precision. <see cref="DateTime"/> ticks are 100ns each, so multiplying
        /// by <see cref="TimeSpan.TicksPerSecond"/> retains all 6 fractional digits Digifinex publishes.
        /// </summary>
        public DateTime GetTimestamp()
        {
            // 1970-01-01T00:00:00Z in DateTime ticks. Hard-coded so the conversion compiles on
            // netstandard2.0, where DateTime.UnixEpoch isn't available.
            const long unixEpochTicks = 621355968000000000L;
            var ticks = (long)Math.Round(Time * TimeSpan.TicksPerSecond) + unixEpochTicks;
            return new DateTime(ticks, DateTimeKind.Utc);
        }
    }
}
