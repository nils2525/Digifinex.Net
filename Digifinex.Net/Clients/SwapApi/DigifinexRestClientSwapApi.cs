using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Digifinex.Net.Clients.MessageHandlers;
using Digifinex.Net.Interfaces.Clients.SwapApi;
using Digifinex.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Clients.SwapApi
{
    /// <inheritdoc cref="IDigifinexRestClientSwapApi" />
    internal partial class DigifinexRestClientSwapApi : RestApiClient<DigifinexEnvironment, DigifinexAuthenticationProvider, DigifinexCredentials>, IDigifinexRestClientSwapApi
    {
        /// <inheritdoc />
        public new DigifinexRestOptions ClientOptions => (DigifinexRestOptions)base.ClientOptions;

        protected override ErrorMapping ErrorMapping { get; } = DigifinexErrors.RestErrorMapping;

        protected override IRestMessageHandler MessageHandler { get; } = new DigifinexRestMessageHandler(DigifinexErrors.RestErrorMapping);

        /// <inheritdoc />
        public string ExchangeName => "Digifinex";

        /// <inheritdoc />
        public IDigifinexRestClientSwapApiExchangeData ExchangeData { get; }

        public DigifinexRestClientSwapApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, DigifinexRestOptions options) :
            base(loggerFactory, DigifinexExchange.ExchangeName, httpClient, options.Environment.RestBaseAddress, options, options.ApiOptions)
        {
            RequestBodyFormat = RequestBodyFormat.Json;
            RequestBodyEmptyContent = string.Empty;

            ExchangeData = new DigifinexRestClientSwapApiExchangeData(this);
        }

        protected override IMessageSerializer CreateSerializer()
            => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DigifinexExchange._serializerContext));

        protected override DigifinexAuthenticationProvider CreateAuthenticationProvider(DigifinexCredentials credentials)
            => new DigifinexAuthenticationProvider(credentials);

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => DigifinexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            return await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }
    }
}
