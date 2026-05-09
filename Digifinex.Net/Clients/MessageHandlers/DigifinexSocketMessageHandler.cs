using System.Text.Json;
using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;

namespace Digifinex.Net.Clients.MessageHandlers
{
    /// <summary>
    /// Routes incoming Digifinex websocket messages to the right query/subscription. Subscribe
    /// and unsubscribe responses are correlated by the echoed <c>id</c> field; live data events
    /// (which omit <c>id</c>) are routed by their <c>method</c> identifier (<c>trades.update</c>,
    /// <c>ticker.update</c>, ...).
    /// </summary>
    internal class DigifinexSocketMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DigifinexExchange._serializerContext);

        public DigifinexSocketMessageHandler()
        {
            // Per-symbol routing for the trade-update push: route by the symbol carried in the
            // params tuple (third element).
            AddTopicMapping<DigifinexTradeUpdateMessage>(x => x.Params.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            // JSON-RPC ack: the response carries the echoed `id`. Force-route by it so the
            // matching query consumes the response even when other concurrent subscriptions are
            // in flight on the same connection. Live data pushes carry `id: null` on the wire,
            // so the not-null constraint lets those frames fall through to the `method` evaluator
            // below instead of being routed by a null id (which has no matching query).
            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id").WithNotNullConstraint(),
                ],
                TypeIdentifierCallback = x => x.FieldValue("id")!,
            },
            // Live data pushes identify themselves via `method`
            // (e.g. `trades.update`, `ticker.update`, `all_ticker.update`).
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("method"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("method")!,
            }
        ];
    }
}
