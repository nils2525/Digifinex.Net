using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Subscription for the Digifinex public <c>trades</c> websocket channel. A single
    /// <c>trades.subscribe</c> call accepts an array of symbols (verified against the live
    /// endpoint - the server replies with one success ack and then pushes <c>trades.update</c>
    /// frames per symbol). Per-message routing uses the symbol carried in the third
    /// <c>params</c> entry so a single subscription serves multiple symbols, each routed to its
    /// own handler.
    /// <para><a href="https://docs.digifinex.com/en-ww/spot/v1/websocket.html" /></para>
    /// </summary>
    internal class DigifinexTradeSubscription : Subscription
    {
        #region Fields
        private readonly Action<DateTime, string?, DigifinexTradeUpdateMessage> _handler;
        private readonly string[] _symbols;
        #endregion

        #region Constructors
        public DigifinexTradeSubscription(
            ILogger logger,
            string[] symbols,
            Action<DateTime, string?, DigifinexTradeUpdateMessage> handler) : base(logger, false)
        {
            _symbols = symbols;
            _handler = handler;
            IndividualSubscriptionCount = Math.Max(1, symbols.Length);

            // Build one route per symbol so the framework's topic-filter can match the symbol
            // carried in `params[2]` (DigifinexSocketMessageHandler maps the topic to
            // `Params.Symbol`). Every route invokes the same handler; the wrapper layer fans the
            // event out to the right per-symbol callback.
            var routes = new List<MessageRoute>();
            foreach (var symbol in symbols)
                routes.Add(MessageRoute.CreateForEvent<DigifinexTradeUpdateMessage>("trades.update", symbol, DoHandleMessage));
            MessageRouter = MessageRouter.Create(routes.ToArray());
        }
        #endregion

        #region Methods
        protected override Query? GetSubQuery(SocketConnection connection)
            => new DigifinexQuery(new DigifinexSocketRequest
            {
                Method = "trades.subscribe",
                Params = _symbols
            }, Authenticated);

        protected override Query? GetUnsubQuery(SocketConnection connection)
            => new DigifinexQuery(new DigifinexSocketRequest
            {
                Method = "trades.unsubscribe",
                Params = _symbols
            }, Authenticated);

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexTradeUpdateMessage message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
        #endregion
    }
}
