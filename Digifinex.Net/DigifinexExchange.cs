using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;
using Digifinex.Net.Converters;

namespace Digifinex.Net
{
    /// <summary>
    /// Digifinex exchange information and configuration
    /// </summary>
    public static class DigifinexExchange
    {
        internal static JsonSerializerContext _serializerContext = JsonSerializerContextCache.GetOrCreate<DigifinexSourceGenerationContext>();

        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Digifinex",
                "Digifinex",
                "https://www.digifinex.com/favicon.ico",
                "https://www.digifinex.com",
                ["https://docs.digifinex.com/"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized
                );

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Digifinex";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "Digifinex";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://www.digifinex.com/favicon.ico";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.digifinex.com/";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://docs.digifinex.com/"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        /// <summary>
        /// Aliases for Digifinex assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = []
        };

        /// <summary>
        /// Format a base and quote asset to a Digifinex recognized symbol (<c>base_quote</c>, lowercase
        /// for <c>/v3/markets</c> and <c>/v3/ticker</c>; uppercase for <c>/v3/spot/symbols</c> and the
        /// websocket channels - the websocket symbol form is used here as the canonical token).
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            return baseAsset + "_" + quoteAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the Digifinex API
        /// </summary>
        public static DigifinexRateLimiters RateLimiter { get; } = new DigifinexRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Digifinex API. Limits sourced from
    /// <see href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />.
    /// </summary>
    public class DigifinexRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send,
        /// so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        internal DigifinexRateLimiters()
#pragma warning restore CS8618
        {
            Initialize();
        }

        private void Initialize()
        {
            // REST: weight-budget of 1200 per minute per scope. The docs phrase the scope as
            // "any [IP|API-KEY|User]" - per-API-key when authenticated, per-IP otherwise. We track
            // per-API-key here; unauthenticated public usage shares the bucket per process.
            // Exceeding triggers a 2-minute lockout for the first 3 violations within 24 hours.
            Rest = new RateLimitGate("Rest")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new LimitItemTypeFilter(RateLimitItemType.Request), 1200, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding));

            // Websocket: per https://docs.digifinex.com/en-ww/swap/v2/websocket.html#websocket
            // (which documents the otherwise-undocumented spot WS contract Digifinex shares with
            // swap):
            //   1. A connection drops after 60s of silence - handled by the periodic
            //      `server.ping` query registered in DigifinexSocketClientSpotApi.
            //   2. A single connection can subscribe to at most 30 public channels - enforced
            //      structurally via `MaxIndividualSubscriptionsPerConnection = 30` on the socket
            //      api client; the framework spawns additional connections automatically when
            //      the cap is reached.
            // No documented per-second message budget. The guards below cap runaway
            // subscribe/unsubscribe and connection-spawn loops without imposing numbers the docs
            // don't claim.
            Socket = new RateLimitGate("Socket")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerConnection, new LimitItemTypeFilter(RateLimitItemType.Request), 50, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 10, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding));

            Rest.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Rest.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            Socket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Socket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }

        internal IRateLimitGate Rest { get; private set; }
        internal IRateLimitGate Socket { get; private set; }
    }
}
