using CryptoExchange.Net.Objects.Options;

namespace Digifinex.Net.Objects.Options
{
    /// <summary>
    /// Options for the DigifinexRestClient
    /// </summary>
    public class DigifinexRestOptions : RestExchangeOptions<DigifinexEnvironment, DigifinexCredentials>
    {
        /// <summary>
        /// Default options for the DigifinexRestClient
        /// </summary>
        internal static DigifinexRestOptions Default { get; set; } = new DigifinexRestOptions
        {
            Environment = DigifinexEnvironment.Live,
            AutoTimestamp = false
        };

        /// <summary>
        /// ctor
        /// </summary>
        public DigifinexRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public RestApiOptions ApiOptions { get; private set; } = new RestApiOptions();

        internal DigifinexRestOptions Set(DigifinexRestOptions targetOptions)
        {
            targetOptions = base.Set<DigifinexRestOptions>(targetOptions);
            targetOptions.ApiOptions = ApiOptions.Set(targetOptions.ApiOptions);
            return targetOptions;
        }
    }
}
