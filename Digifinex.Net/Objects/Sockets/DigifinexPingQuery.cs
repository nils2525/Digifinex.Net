using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Application-level heartbeat for the Digifinex websocket. Sends
    /// <c>{"id":N,"method":"server.ping","params":[]}</c>; the server answers with
    /// <c>{"error":null,"result":"pong","id":N}</c>. The connection is dropped after 60 seconds
    /// of silence, so the periodic registration in <see cref="Clients.SpotApi.DigifinexSocketClientSpotApi"/>
    /// fires this query at a sub-60s interval.
    /// </summary>
    internal class DigifinexPingQuery : Query<DigifinexPingResponse>
    {
        public DigifinexPingQuery() : base(BuildRequest(), false, 0)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            // Server echoes the client-supplied id; route the pong by that id, no handler needed
            // (the framework's request-response correlation completes the query for us).
            MessageRouter = MessageRouter.CreateWithoutHandler<DigifinexPingResponse>(
                ((DigifinexSocketRequest)Request).Id.ToString());
        }

        private static DigifinexSocketRequest BuildRequest()
            => new DigifinexSocketRequest
            {
                Id = ExchangeHelpers.NextId(),
                Method = "server.ping",
                Params = Array.Empty<string>()
            };
    }
}
