using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;

namespace Digifinex.Net
{
    /// <summary>
    /// HMAC-SHA256 authentication for the Digifinex REST API.
    /// <para>
    /// The signing input is the URL-encoded ASCII-sorted parameter string. For GET requests this
    /// is the query string; for POST it is the form-encoded body. When both are present the docs
    /// require <c>query &amp; body</c>, query first. The HMAC-SHA256 signature is rendered as
    /// lowercase hex and submitted alongside <c>ACCESS-KEY</c> + <c>ACCESS-TIMESTAMP</c> headers.
    /// See <see href="https://docs.digifinex.com/en-ww/spot/v3/rest.html" />.
    /// </para>
    /// </summary>
    internal class DigifinexAuthenticationProvider : AuthenticationProvider<DigifinexCredentials, DigifinexCredentials>
    {
        #region Constructors
        public DigifinexAuthenticationProvider(DigifinexCredentials credentials) : base(credentials, credentials)
        {
        }
        #endregion

        #region Methods
        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            var timestamp = (long.Parse(GetMillisecondTimestamp(apiClient)) / 1000).ToString();

            var queryString = requestConfig.GetQueryString(true);
            var bodyString = string.Empty;
            if (requestConfig.BodyParameters?.Any() == true)
            {
                bodyString = requestConfig.BodyParameters.ToFormData();
                requestConfig.SetBodyContent(bodyString);
                requestConfig.BodyFormat = RequestBodyFormat.FormData;
            }

            string signSource;
            if (!string.IsNullOrEmpty(queryString) && !string.IsNullOrEmpty(bodyString))
                signSource = queryString + "&" + bodyString;
            else if (!string.IsNullOrEmpty(queryString))
                signSource = queryString;
            else
                signSource = bodyString;

            var signature = SignHMACSHA256(signSource, SignOutputType.Hex);

            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers["ACCESS-KEY"] = Credential.Key;
            requestConfig.Headers["ACCESS-SIGN"] = signature;
            requestConfig.Headers["ACCESS-TIMESTAMP"] = timestamp;
        }

        /// <summary>
        /// Compute the HMAC-SHA256 signature for the given input using the configured API secret.
        /// Used by the websocket authentication flow.
        /// </summary>
        internal string Sign(string data) => SignHMACSHA256(data, SignOutputType.Hex);
        #endregion
    }
}
