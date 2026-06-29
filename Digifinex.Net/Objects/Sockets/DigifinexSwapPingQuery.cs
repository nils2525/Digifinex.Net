using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default.Routing;
using Digifinex.Net.Objects.Models.Socket;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Application-level heartbeat for the Digifinex swap websocket.
    /// </summary>
    internal class DigifinexSwapPingQuery : Query<DigifinexSwapPingResponse>
    {
        public DigifinexSwapPingQuery() : base(BuildRequest(), false, 0)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageRouter = MessageRouter.CreateForQuery<DigifinexSwapPingResponse>(
                ((DigifinexSwapSocketRequest)Request).Id.ToString(),
                (connection, receiveTime, originalData, message) => CallResult.Ok(message, originalData));
        }

        private static DigifinexSwapSocketRequest BuildRequest()
            => new DigifinexSwapSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Event = "server.ping"
            };
    }
}
