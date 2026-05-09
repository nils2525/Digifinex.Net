using System.Net.Http.Headers;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;

namespace Digifinex.Net.Clients.MessageHandlers
{
    /// <summary>
    /// REST message handler for Digifinex. Digifinex mixes two success conventions across
    /// endpoints: most return <c>"code":0</c>, while currency/deposit/withdraw endpoints return
    /// <c>"code":200</c>. Any other numeric value is treated as an error and the
    /// <c>msg</c>/<c>message</c> field (if present) is surfaced as the human-readable reason.
    /// </summary>
    internal class DigifinexRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;
        public override bool RequiresSeekableStream => true;
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DigifinexExchange._serializerContext);

        public DigifinexRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        private Error ParseErrorInternal(JsonElement rootElement)
        {
            var code = rootElement.TryGetProperty("code", out var codeProp) ? codeProp.ToString() : "?";

            string? reason = null;
            if (rootElement.TryGetProperty("msg", out var msgProp))
                reason = msgProp.GetString();
            reason ??= rootElement.TryGetProperty("message", out var messageProp) ? messageProp.GetString() : null;

            return new ServerError(code, _errorMapping.GetErrorInfo(code, reason));
        }

        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (parseError, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (parseError != null)
                return parseError;

            return ParseErrorInternal(document!.RootElement);
        }

        public override async ValueTask<Error?> CheckForErrorResponse(RequestDefinition request, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (parseError, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (parseError != null)
                return parseError;

            if (document!.RootElement.ValueKind is JsonValueKind.Array)
                return null;

            // Digifinex mixes two success conventions: most endpoints use `code:0` while
            // currency/deposit/withdraw endpoints use `code:200`. Treat both as success; any
            // other value is an error. A missing `code` (some endpoints return only `{"data":...}`)
            // is also treated as success - the wrapper-side deserialization will still fail loudly
            // if the body shape doesn't match the expected model.
            if (document.RootElement.TryGetProperty("code", out var codeProp)
                && codeProp.ValueKind == JsonValueKind.Number
                && codeProp.TryGetInt32(out var codeInt)
                && codeInt != 0
                && codeInt != 200)
            {
                return ParseErrorInternal(document.RootElement);
            }

            return await base.CheckForErrorResponse(request, responseHeaders, responseStream).ConfigureAwait(false);
        }
    }
}
