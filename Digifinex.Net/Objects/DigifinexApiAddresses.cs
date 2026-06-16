namespace Digifinex.Net.Objects
{
    /// <summary>
    /// Api addresses usable for the Digifinex clients
    /// </summary>
    public class DigifinexApiAddresses
    {
        /// <summary>
        /// The address used by the DigifinexRestClient for the rest API (<c>https://openapi.digifinex.com</c>)
        /// </summary>
        public string RestClientAddress { get; set; } = "";

        /// <summary>
        /// The address used by the DigifinexSocketClient for the spot websocket API (<c>wss://openapi.digifinex.com/ws/v1/</c>)
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The address used by the DigifinexSocketClient for the swap websocket API (<c>wss://openapi.digifinex.com/swap_ws/v2/</c>)
        /// </summary>
        public string SwapSocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Digifinex API
        /// </summary>
        public static DigifinexApiAddresses Default = new DigifinexApiAddresses
        {
            RestClientAddress = "https://openapi.digifinex.com",
            SocketClientAddress = "wss://openapi.digifinex.com/ws/v1/",
            SwapSocketClientAddress = "wss://openapi.digifinex.com/swap_ws/v2/"
        };
    }
}
