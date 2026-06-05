using System.Collections.Concurrent;
using Digifinex.Net.Interfaces.Clients;
using Digifinex.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digifinex.Net.Clients
{
    /// <inheritdoc />
    public class DigifinexUserClientProvider : IDigifinexUserClientProvider
    {
        private ConcurrentDictionary<string, IDigifinexRestClient> _restClients = new ConcurrentDictionary<string, IDigifinexRestClient>();
        private ConcurrentDictionary<string, IDigifinexSocketClient> _socketClients = new ConcurrentDictionary<string, IDigifinexSocketClient>();

        private readonly IOptions<DigifinexRestOptions> _restOptions;
        private readonly IOptions<DigifinexSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <inheritdoc />
        public string ExchangeName => DigifinexExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public DigifinexUserClientProvider(Action<DigifinexOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public DigifinexUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<DigifinexRestOptions> restOptions,
            IOptions<DigifinexSocketOptions> socketOptions)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.Timeout = restOptions.Value.RequestTimeout;
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, DigifinexCredentials credentials, DigifinexEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IDigifinexRestClient GetRestClient(string userIdentifier, DigifinexCredentials? credentials = null, DigifinexEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IDigifinexSocketClient GetSocketClient(string userIdentifier, DigifinexCredentials? credentials = null, DigifinexEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IDigifinexRestClient CreateRestClient(string userIdentifier, DigifinexCredentials? credentials, DigifinexEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new DigifinexRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients[userIdentifier] = client;
            }
            return client;
        }

        private IDigifinexSocketClient CreateSocketClient(string userIdentifier, DigifinexCredentials? credentials, DigifinexEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new DigifinexSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients[userIdentifier] = client;
            }
            return client;
        }

        private IOptions<DigifinexRestOptions> SetRestEnvironment(DigifinexEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new DigifinexRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<DigifinexSocketOptions> SetSocketEnvironment(DigifinexEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new DigifinexSocketOptions();
            var socketOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
