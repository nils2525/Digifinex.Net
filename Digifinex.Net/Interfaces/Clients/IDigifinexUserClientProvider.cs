using CryptoExchange.Net.Interfaces;

namespace Digifinex.Net.Interfaces.Clients
{
    /// <summary>
    /// Provider for clients with credentials for specific users
    /// </summary>
    public interface IDigifinexUserClientProvider : IExchangeService
    {
        /// <summary>
        /// Initialize a client for the specified user identifier.
        /// </summary>
        /// <param name="userIdentifier">The identifier for the user</param>
        /// <param name="credentials">The credentials for the user</param>
        /// <param name="environment">The environment to use</param>
        void InitializeUserClient(string userIdentifier, DigifinexCredentials credentials, DigifinexEnvironment? environment = null);

        /// <summary>
        /// Reset the cached clients for a user.
        /// </summary>
        public void ClearUserClients(string userIdentifier);

        /// <summary>
        /// Get the Rest client for a specific user.
        /// </summary>
        IDigifinexRestClient GetRestClient(string userIdentifier, DigifinexCredentials? credentials = null, DigifinexEnvironment? environment = null);

        /// <summary>
        /// Get the Socket client for a specific user.
        /// </summary>
        IDigifinexSocketClient GetSocketClient(string userIdentifier, DigifinexCredentials? credentials = null, DigifinexEnvironment? environment = null);
    }
}
