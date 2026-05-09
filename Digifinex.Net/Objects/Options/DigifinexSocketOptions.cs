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

        internal DigifinexSocketOptions Set(DigifinexSocketOptions targetOptions)
        {
            targetOptions = base.Set<DigifinexSocketOptions>(targetOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
