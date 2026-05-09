using Digifinex.Net.Objects.Models.Socket;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Digifinex.Net.Converters
{
    /// <summary>
    /// Reads the heterogeneous <c>params</c> array of a <c>trades.update</c> push message and
    /// projects it onto a <see cref="DigifinexTradeUpdate"/>. Wire shape:
    /// <c>[clean, [trades], "BTC_USDT"]</c>.
    /// </summary>
    internal class DigifinexTradeUpdateConverter : JsonConverter<DigifinexTradeUpdate>
    {
        public override DigifinexTradeUpdate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException($"Expected StartArray for DigifinexTradeUpdate, got {reader.TokenType}");

            var result = new DigifinexTradeUpdate();

            // [0] clean flag
            reader.Read();
            result.Clean = reader.TokenType == JsonTokenType.True;

            // [1] trades array
            reader.Read();
            result.Trades = JsonSerializer.Deserialize<DigifinexTradeEntry[]>(ref reader, options) ?? Array.Empty<DigifinexTradeEntry>();

            // [2] symbol
            reader.Read();
            result.Symbol = reader.GetString() ?? string.Empty;

            // EndArray
            reader.Read();
            return result;
        }

        public override void Write(Utf8JsonWriter writer, DigifinexTradeUpdate value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteBooleanValue(value.Clean);
            JsonSerializer.Serialize(writer, value.Trades, options);
            writer.WriteStringValue(value.Symbol);
            writer.WriteEndArray();
        }
    }
}
