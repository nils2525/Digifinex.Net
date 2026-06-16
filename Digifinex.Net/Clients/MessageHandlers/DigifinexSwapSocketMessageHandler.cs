using System.Text.Json;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Digifinex.Net.Objects.Models.Socket;

namespace Digifinex.Net.Clients.MessageHandlers
{
    /// <summary>
    /// Routes incoming Digifinex swap websocket messages.
    /// </summary>
    internal class DigifinexSwapSocketMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DigifinexExchange._serializerContext);

        public DigifinexSwapSocketMessageHandler()
        {
            AddTopicMapping<DigifinexSwapTradeUpdateMessage>(x => x.Data.FirstOrDefault()?.InstrumentId);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id").WithNotNullConstraint(),
                ],
                TypeIdentifierCallback = x => x.FieldValue("id")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("event")!,
            }
        ];
    }
}
