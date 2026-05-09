using CryptoExchange.Net.Authentication;

namespace Digifinex.Net
{
    /// <summary>
    /// Digifinex API credentials
    /// </summary>
    public class DigifinexCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials
        /// </summary>
        public DigifinexCredentials() { }

        /// <summary>
        /// Create new credentials providing HMAC credentials
        /// </summary>
        /// <param name="credential">HMAC Credentials</param>
        public DigifinexCredentials(HMACCredential credential) : base(credential.Key, credential.Secret)
        {
        }

        /// <summary>
        /// Create new credentials providing credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public DigifinexCredentials(string key, string secret) : base(key, secret)
        {
        }

        /// <summary>
        /// Specify the HMAC credentials
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public DigifinexCredentials WithHMAC(string key, string secret)
        {
            if (!string.IsNullOrEmpty(Key)) throw new InvalidOperationException("Credentials already set");

            Key = key;
            Secret = secret;
            return this;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new DigifinexCredentials(this);
    }
}
