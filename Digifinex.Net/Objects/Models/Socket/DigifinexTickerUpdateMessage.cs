using Digifinex.Net.Converters;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Ticker update push received from the Digifinex <c>ticker.subscribe</c> /
    /// <c>all_ticker.subscribe</c> channel. Wire shape:
    /// <c>{"method":"ticker.update","params":[[ticker, ...]]}</c> for the per-symbol channel; the
    /// all-symbols channel batches multiple tickers in a single push. The custom converter
    /// flattens the params array down to a typed <see cref="Tickers"/> collection.
    /// </summary>
    [JsonConverter(typeof(DigifinexTickerUpdateConverter))]
    public record DigifinexTickerUpdateMessage
    {
        /// <summary>
        /// Ticker entries carried by this update.
        /// </summary>
        public DigifinexTickerUpdate[] Tickers { get; set; } = Array.Empty<DigifinexTickerUpdate>();
    }
}
