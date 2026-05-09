using Digifinex.Net.Objects;
using CryptoExchange.Net.Objects;

namespace Digifinex.Net
{
    /// <summary>
    /// Digifinex environments
    /// </summary>
    public class DigifinexEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest client address
        /// </summary>
        public string RestBaseAddress { get; }

        /// <summary>
        /// Spot Socket API address (<c>wss://openapi.digifinex.com/ws/v1/</c>)
        /// </summary>
        public string SocketBaseAddress { get; }

        internal DigifinexEnvironment(string name,
            string restBaseAddress,
            string socketBaseAddress) : base(name)
        {
            RestBaseAddress = restBaseAddress;
            SocketBaseAddress = socketBaseAddress;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public DigifinexEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618
        { }

        /// <summary>
        /// Get the Digifinex environment by name
        /// </summary>
        public static DigifinexEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static DigifinexEnvironment Live { get; }
            = new DigifinexEnvironment(TradeEnvironmentNames.Live,
                                       DigifinexApiAddresses.Default.RestClientAddress,
                                       DigifinexApiAddresses.Default.SocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        public static DigifinexEnvironment CreateCustom(
                        string name,
                        string restAddress,
                        string socketAddress)
            => new DigifinexEnvironment(name, restAddress, socketAddress);
    }
}
