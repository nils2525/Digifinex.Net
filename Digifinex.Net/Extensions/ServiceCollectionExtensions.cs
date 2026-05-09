using Digifinex.Net;
using Digifinex.Net.Clients;
using Digifinex.Net.Interfaces.Clients;
using Digifinex.Net.Objects.Options;
using CryptoExchange.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services such as the IDigifinexRestClient and IDigifinexSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        public static IServiceCollection AddDigifinex(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new DigifinexOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;

            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid configuration provided", ex);
            }

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? DigifinexEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? DigifinexEnvironment.Live.Name;
            options.Rest.Environment = DigifinexEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = DigifinexEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return services.AddDigifinexCore(options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IDigifinexRestClient and IDigifinexSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the Digifinex services</param>
        public static IServiceCollection AddDigifinex(
            this IServiceCollection services,
            Action<DigifinexOptions>? optionsDelegate = null)
        {
            var options = new DigifinexOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? DigifinexEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? DigifinexEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return services.AddDigifinexCore(options.SocketClientLifeTime);
        }

        private static IServiceCollection AddDigifinexCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IDigifinexRestClient, DigifinexRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DigifinexRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new DigifinexRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<DigifinexRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<DigifinexRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IDigifinexSocketClient), x => { return new DigifinexSocketClient(x.GetRequiredService<IOptions<DigifinexSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddSingleton<IDigifinexUserClientProvider, DigifinexUserClientProvider>(x =>
            new DigifinexUserClientProvider(
                x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(IDigifinexRestClient).Name),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<DigifinexRestOptions>>(),
                x.GetRequiredService<IOptions<DigifinexSocketOptions>>()));

            return services;
        }
    }
}
