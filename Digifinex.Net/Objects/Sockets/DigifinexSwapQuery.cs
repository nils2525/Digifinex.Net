using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Digifinex.Net.Objects.Models.Socket;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Swap websocket subscribe/unsubscribe query.
    /// </summary>
    internal class DigifinexSwapQuery : Query<DigifinexSwapSubscriptionResponse>
    {
        public DigifinexSwapQuery(DigifinexSwapSocketRequest request, bool authenticated, int weight = 1) : base(AssignRequestId(request), authenticated, weight)
        {
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<DigifinexSwapSubscriptionResponse>(
                request.Id.ToString(), HandleMessage);
        }

        public CallResult<DigifinexSwapSubscriptionResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexSwapSubscriptionResponse message)
        {
            if (message.Code != 1)
            {
                var code = message.Code.ToString();
                var info = DigifinexErrors.RestErrorMapping.GetErrorInfo(code, message.Message);
                return new CallResult<DigifinexSwapSubscriptionResponse>(new ServerError(code, info));
            }

            return new CallResult<DigifinexSwapSubscriptionResponse>(message, originalData, null);
        }

        private static DigifinexSwapSocketRequest AssignRequestId(DigifinexSwapSocketRequest request)
        {
            request.Id = ExchangeHelpers.NextId();
            return request;
        }
    }
}
