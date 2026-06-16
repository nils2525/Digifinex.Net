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
    /// <inheritdoc cref="IDigifinexSocketClient" />
    public class DigifinexSocketClient : BaseSocketClient<DigifinexEnvironment, DigifinexCredentials>, IDigifinexSocketClient
    {
        /// <inheritdoc />
        public IDigifinexSocketClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IDigifinexSocketClientSwapApi SwapApi { get; }

        #region ctor

        /// <summary>
        /// Create a new instance of the DigifinexSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public DigifinexSocketClient(Action<DigifinexSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of the DigifinexSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public DigifinexSocketClient(IOptions<DigifinexSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Digifinex")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new DigifinexSocketClientSpotApi(_logger, options.Value));
            SwapApi = AddApiClient(new DigifinexSocketClientSwapApi(_logger, options.Value));
        }
        #endregion

        /// <inheritdoc />
        public override void SetApiCredentials(DigifinexCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
            SwapApi.SetApiCredentials(credentials);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<DigifinexSocketOptions> optionsDelegate)
        {
            DigifinexSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
