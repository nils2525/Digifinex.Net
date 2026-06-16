using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Digifinex.Net.Objects.Models.Socket;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Subscription for the Digifinex swap public <c>trades</c> channel.
    /// </summary>
    internal class DigifinexSwapTradeSubscription : Subscription
    {
        private readonly Action<DateTime, string?, DigifinexSwapTradeUpdateMessage> _handler;
        private readonly string[] _instrumentIds;

        public DigifinexSwapTradeSubscription(
            ILogger logger,
            string[] instrumentIds,
            Action<DateTime, string?, DigifinexSwapTradeUpdateMessage> handler) : base(logger, false)
        {
            _instrumentIds = instrumentIds;
            _handler = handler;
            IndividualSubscriptionCount = Math.Max(1, instrumentIds.Length);

            var routes = new List<MessageRoute>();
            foreach (var instrumentId in instrumentIds)
                routes.Add(MessageRoute<DigifinexSwapTradeUpdateMessage>.CreateWithTopicFilter("trades.update", instrumentId, DoHandleMessage));
            MessageRouter = MessageRouter.Create(routes.ToArray());
        }

        protected override Query? GetSubQuery(SocketConnection connection)
            => new DigifinexSwapQuery(new DigifinexSwapSocketRequest
            {
                Event = "trades.subscribe",
                InstrumentIds = _instrumentIds
            }, Authenticated);

        protected override Query? GetUnsubQuery(SocketConnection connection)
            => new DigifinexSwapQuery(new DigifinexSwapSocketRequest
            {
                Event = "trades.unsubscribe",
                InstrumentIds = _instrumentIds
            }, Authenticated);

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexSwapTradeUpdateMessage message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.SuccessResult;
        }
    }
}
