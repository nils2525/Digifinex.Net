using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Digifinex.Net.Objects.Models.Socket;
using Microsoft.Extensions.Logging;

namespace Digifinex.Net.Objects.Sockets
{
    /// <summary>
    /// Subscription for the swap <c>all_ticker</c> public channel.
    /// </summary>
    internal class DigifinexSwapAllTickerSubscription : Subscription
    {
        private readonly Action<DateTime, string?, DigifinexSwapTickerUpdateMessage> _handler;

        /// <summary>Initializes the swap all-ticker subscription.</summary>
        /// <param name="logger">The logger used for socket diagnostics.</param>
        /// <param name="handler">The handler invoked for each all-ticker update.</param>
        public DigifinexSwapAllTickerSubscription(
            ILogger logger,
            Action<DateTime, string?, DigifinexSwapTickerUpdateMessage> handler)
            : base(logger, false)
        {
            _handler = handler;
            MessageRouter = MessageRouter.CreateForEvent<DigifinexSwapTickerUpdateMessage>(["all_ticker.update"], DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
            => new DigifinexSwapQuery(new DigifinexSwapSocketRequest
            {
                Event = "all_ticker.subscribe"
            }, Authenticated);

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
            => new DigifinexSwapQuery(new DigifinexSwapSocketRequest
            {
                Event = "all_ticker.unsubscribe"
            }, Authenticated);

        /// <summary>Dispatches a routed all-ticker message.</summary>
        /// <param name="connection">The socket connection that received the message.</param>
        /// <param name="receiveTime">The local receive timestamp.</param>
        /// <param name="originalData">The original serialized message, when retained.</param>
        /// <param name="message">The deserialized all-ticker update.</param>
        /// <returns>A successful dispatch result.</returns>
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DigifinexSwapTickerUpdateMessage message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
