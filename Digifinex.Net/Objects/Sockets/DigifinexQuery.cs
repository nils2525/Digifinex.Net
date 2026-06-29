using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// JSON-RPC subscribe/unsubscribe query for the Digifinex websocket APIs. The server echoes
    /// the client-supplied <c>id</c> in both the success
    /// (<c>{"error":null,"result":{"status":"success"},"id":N}</c>) and error
    /// (<c>{"error":{"code":...,"message":"..."},"result":null,"id":N}</c>) response, so the
    /// query is routed back to the originator by id.
    /// </summary>
    internal class DigifinexQuery : Query<DigifinexSubscriptionResponse>
    {
        public DigifinexQuery(DigifinexSocketRequest request, bool authenticated, int weight = 1) : base(AssignRequestId(request), authenticated, weight)
        {
            MessageRouter = MessageRouter.CreateForQuery<DigifinexSubscriptionResponse>(
                request.Id.ToString(), HandleMessage);
        }

        public CallResult<DigifinexSubscriptionResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexSubscriptionResponse message)
        {
            if (message.Error != null)
            {
                var code = message.Error.Code.ToString();
                var info = DigifinexErrors.RestErrorMapping.GetErrorInfo(code, message.Error.Message);
                return CallResult.Fail<DigifinexSubscriptionResponse>(new ServerError(code, info));
            }

            return CallResult.Ok(message, originalData);
        }

        private static DigifinexSocketRequest AssignRequestId(DigifinexSocketRequest request)
        {
            request.Id = ExchangeHelpers.NextId();
            return request;
        }
    }
}
