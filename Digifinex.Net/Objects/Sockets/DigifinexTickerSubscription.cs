using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Subscription for the Digifinex per-symbol <c>ticker</c> websocket channel
    /// (<c>ticker.subscribe</c>). Accepts an explicit list of symbols.
    /// </summary>
    internal class DigifinexTickerSubscription : Subscription
    {
        private readonly Action<DateTime, string?, DigifinexTickerUpdateEnvelope> _handler;
        private readonly string[] _symbols;

        public DigifinexTickerSubscription(
            ILogger logger,
            string[] symbols,
            Action<DateTime, string?, DigifinexTickerUpdateEnvelope> handler) : base(logger, false)
        {
            _symbols = symbols;
            _handler = handler;

            MessageRouter = MessageRouter.CreateWithoutTopicFilter<DigifinexTickerUpdateEnvelope>(["ticker.update"], DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
            => new DigifinexQuery(new DigifinexSocketRequest
            {
                Method = "ticker.subscribe",
                Params = _symbols
            }, Authenticated);

        protected override Query? GetUnsubQuery(SocketConnection connection)
            => new DigifinexQuery(new DigifinexSocketRequest
            {
                Method = "ticker.unsubscribe",
                Params = _symbols
            }, Authenticated);

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexTickerUpdateEnvelope message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.SuccessResult;
        }
    }
}
