using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Subscription for the Digifinex <c>all_ticker</c> websocket channel
    /// (<c>all_ticker.subscribe</c>). Empty <c>params</c> - one subscribe covers every currently
    /// listed market. Pushes arrive as <c>all_ticker.update</c> envelopes throttled to ~1Hz
    /// server-side; each push carries a partial slice of currently-active tickers and symbols
    /// accumulate across pushes.
    /// </summary>
    internal class DigifinexAllTickerSubscription : Subscription
    {
        private readonly Action<DateTime, string?, DigifinexTickerUpdateEnvelope> _handler;

        public DigifinexAllTickerSubscription(
            ILogger logger,
            Action<DateTime, string?, DigifinexTickerUpdateEnvelope> handler) : base(logger, false)
        {
            _handler = handler;

            MessageRouter = MessageRouter.CreateForEvent<DigifinexTickerUpdateEnvelope>(["all_ticker.update"], DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
            => new DigifinexQuery(new DigifinexSocketRequest
            {
                Method = "all_ticker.subscribe",
                Params = Array.Empty<string>()
            }, Authenticated);

        protected override Query? GetUnsubQuery(SocketConnection connection)
            => new DigifinexQuery(new DigifinexSocketRequest
            {
                Method = "all_ticker.unsubscribe",
                Params = Array.Empty<string>()
            }, Authenticated);

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexTickerUpdateEnvelope message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
