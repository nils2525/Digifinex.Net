using CryptoExchange.Net.Objects.Options;

namespace Digifinex.Net.Objects.Options
{
    /// <summary>
    /// Options for the DigifinexSocketClient.
    /// </summary>
    public class DigifinexSocketOptions : SocketExchangeOptions<DigifinexEnvironment, DigifinexCredentials>
    {
        /// <summary>
        /// Default options for the DigifinexSocketClient
        /// </summary>
        internal static DigifinexSocketOptions Default { get; set; } = new DigifinexSocketOptions
        {
            Environment = DigifinexEnvironment.Live,
            SocketSubscriptionsCombineTarget = 30,
        };

        /// <summary>
        /// ctor
        /// </summary>
        public DigifinexSocketOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Options for the Spot Socket API (<c>wss://openapi.digifinex.com/ws/v1/</c>)
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        /// <summary>
        /// Options for the Swap Socket API (<c>wss://openapi.digifinex.com/swap_ws/v2/</c>)
        /// </summary>
        public SocketApiOptions SwapOptions { get; private set; } = new SocketApiOptions();

        /// <summary>
        /// Maximum lifetime of a single WebSocket connection before it is proactively recycled.
        /// Digifinex closes WS connections server-side at the 2-hour mark (undocumented; the
        /// public docs claim 24h). Defaults to 110min to stay under the observed cap with margin.
        /// Set to <see cref="TimeSpan.Zero"/> to disable.
        /// </summary>
        public TimeSpan MaxConnectionLifetime { get; set; } = TimeSpan.FromMinutes(110);

        internal DigifinexSocketOptions Set(DigifinexSocketOptions targetOptions)
        {
            targetOptions = base.Set<DigifinexSocketOptions>(targetOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            targetOptions.SwapOptions = SwapOptions.Set(targetOptions.SwapOptions);
            targetOptions.MaxConnectionLifetime = MaxConnectionLifetime;
            return targetOptions;
        }
    }
}
