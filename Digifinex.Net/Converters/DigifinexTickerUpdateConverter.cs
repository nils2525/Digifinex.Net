using Digifinex.Net.Objects.Models.Socket;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Converters
{
    /// <summary>
    /// Reads the <c>params</c> array of a <c>ticker.update</c> / <c>all_ticker.update</c> push
    /// message and projects it onto a <see cref="DigifinexTickerUpdateMessage"/>. Wire shape is
    /// an array containing a single nested array of ticker objects:
    /// <c>[[{ticker}, {ticker}, ...]]</c>.
    /// </summary>
    internal class DigifinexTickerUpdateConverter : JsonConverter<DigifinexTickerUpdateMessage>
    {
        public override DigifinexTickerUpdateMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException($"Expected StartArray for DigifinexTickerUpdateMessage, got {reader.TokenType}");

            var result = new DigifinexTickerUpdateMessage();
            var tickers = new List<DigifinexTickerUpdate>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var inner = JsonSerializer.Deserialize<DigifinexTickerUpdate[]>(ref reader, options);
                    if (inner != null)
                        tickers.AddRange(inner);
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var single = JsonSerializer.Deserialize<DigifinexTickerUpdate>(ref reader, options);
                    if (single != null)
                        tickers.Add(single);
                }
                else
                {
                    reader.Skip();
                }
            }

            result.Tickers = tickers.ToArray();
            return result;
        }

        public override void Write(Utf8JsonWriter writer, DigifinexTickerUpdateMessage value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            JsonSerializer.Serialize(writer, value.Tickers, options);
            writer.WriteEndArray();
        }
    }
}
