using Digifinex.Net.Clients.SpotApi;
using Digifinex.Net.Clients.SwapApi;
using Digifinex.Net.Interfaces.Clients;
using Digifinex.Net.Interfaces.Clients.SpotApi;
using Digifinex.Net.Interfaces.Clients.SwapApi;
using Digifinex.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digifinex.Net.Clients
{
    /// <inheritdoc cref="IDigifinexRestClient" />
    public class DigifinexRestClient : BaseRestClient<DigifinexEnvironment, DigifinexCredentials>, IDigifinexRestClient
    {
        /// <inheritdoc />
        public IDigifinexRestClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IDigifinexRestClientSwapApi SwapApi { get; }

        #region ctor
        /// <summary>
        /// Create a new instance of the DigifinexRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public DigifinexRestClient(Action<DigifinexRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the DigifinexRestClient
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public DigifinexRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<DigifinexRestOptions> options)
            : base(loggerFactory, "Digifinex")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new DigifinexRestClientSpotApi(_logger, httpClient, options.Value));
            SwapApi = AddApiClient(new DigifinexRestClientSwapApi(_logger, httpClient, options.Value));
        }
        #endregion

        #region methods
        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<DigifinexRestOptions> optionsDelegate)
        {
            DigifinexRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
        #endregion
    }
}
