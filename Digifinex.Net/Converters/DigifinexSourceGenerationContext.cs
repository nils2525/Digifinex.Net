using System.Text.Json.Serialization;
using Digifinex.Net.Objects.Models;
using Digifinex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Objects;

namespace Digifinex.Net.Converters
{
    [JsonSerializable(typeof(DigifinexServerTime))]
    [JsonSerializable(typeof(DigifinexMarket))]
    [JsonSerializable(typeof(DigifinexMarket[]))]
    [JsonSerializable(typeof(DigifinexMarketsResponse))]
    [JsonSerializable(typeof(DigifinexSymbol))]
    [JsonSerializable(typeof(DigifinexSymbol[]))]
    [JsonSerializable(typeof(DigifinexSymbolsResponse))]
    [JsonSerializable(typeof(DigifinexCurrency))]
    [JsonSerializable(typeof(DigifinexCurrency[]))]
    [JsonSerializable(typeof(DigifinexCurrenciesResponse))]
    [JsonSerializable(typeof(DigifinexTicker))]
    [JsonSerializable(typeof(DigifinexTicker[]))]
    [JsonSerializable(typeof(DigifinexTickerResponse))]
    [JsonSerializable(typeof(DigifinexSwapInstrumentsResponse))]
    [JsonSerializable(typeof(DigifinexSwapInstrument))]
    [JsonSerializable(typeof(DigifinexSwapInstrument[]))]
    [JsonSerializable(typeof(DigifinexSwapOpenMaxLimit))]
    [JsonSerializable(typeof(DigifinexSwapOpenMaxLimit[]))]
    [JsonSerializable(typeof(DigifinexSwapTradesResponse))]
    [JsonSerializable(typeof(DigifinexSwapTrade))]
    [JsonSerializable(typeof(DigifinexSwapTrade[]))]
    [JsonSerializable(typeof(DigifinexPingResponse))]

    [JsonSerializable(typeof(DigifinexSocketRequest))]
    [JsonSerializable(typeof(DigifinexSwapSocketRequest))]
    [JsonSerializable(typeof(DigifinexSubscriptionResponse))]
    [JsonSerializable(typeof(DigifinexSwapSubscriptionResponse))]
    [JsonSerializable(typeof(DigifinexSocketResult))]
    [JsonSerializable(typeof(DigifinexSocketError))]
    [JsonSerializable(typeof(DigifinexSwapPingResponse))]
    [JsonSerializable(typeof(DigifinexTradeEntry))]
    [JsonSerializable(typeof(DigifinexTradeEntry[]))]
    [JsonSerializable(typeof(DigifinexTradeUpdate))]
    [JsonSerializable(typeof(DigifinexTradeUpdateMessage))]
    [JsonSerializable(typeof(DigifinexSwapTradeUpdateMessage))]
    [JsonSerializable(typeof(DigifinexTickerUpdate))]
    [JsonSerializable(typeof(DigifinexTickerUpdate[]))]
    [JsonSerializable(typeof(DigifinexTickerUpdateMessage))]
    [JsonSerializable(typeof(DigifinexTickerUpdateEnvelope))]

    [JsonSerializable(typeof(IDictionary<string, object>))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(string[]))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(Parameters))]
    internal partial class DigifinexSourceGenerationContext : JsonSerializerContext
    { }
}
