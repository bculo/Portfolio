using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Infrastracture.Clients;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.UnitTests.Infrastracture
{
    public class CoinMarketCapClientTests
    {
        private const string VALID_SYMBOL = "btc";
        private const string INVALID_SYMBOL = "DRAGON123";

        [Fact]
        public async Task FetchData_Should_Return_Instance_When_Valid_Symbol_Provided()
        {
            var service = Build(VALID_SYMBOL);

            var response = await service.FetchData(VALID_SYMBOL);

            Assert.NotNull(response);

            var itemDictionary = response.Data;

            var dictionaryKey = itemDictionary.ContainsKey(VALID_SYMBOL) ? VALID_SYMBOL : VALID_SYMBOL.ToUpper();

            var finalItemsCollection = itemDictionary[dictionaryKey];

            Assert.Contains(finalItemsCollection, i => i.Symbol.ToLower() == VALID_SYMBOL.ToLower());
        }

        [Fact]
        public async Task FetchData_Should_Return_Null_When_Invalid_Symbol_Provided()
        {
            var service = Build(INVALID_SYMBOL);

            var response = await service.FetchData(INVALID_SYMBOL);

            Assert.Null(response);
        }

        public ICryptoInfoService Build(string symbol)
        {
            var infoOptions = new CryptoInfoApiOptions
            {
                ApiKey = "<COINMARKETCAP_KEY>",
                BaseUrl = "https://pro-api.coinmarketcap.com/v2/cryptocurrency/",
                HeaderKey = "X-CMC_PRO_API_KEY"
            };

            var options = Options.Create(infoOptions);

            var handler = new MockHttpMessageHandler();

            if(symbol == VALID_SYMBOL)
            {
                string btcResponseJson = File.ReadAllText("./Infrastracture/btc-response.json");
                handler.When("https://pro-api.coinmarketcap.com/v2/cryptocurrency/info?symbol=btc")
                    .Respond("application/json", btcResponseJson);
            }
            else
            {
                handler.When("https://pro-api.coinmarketcap.com/v2/cryptocurrency/*")
                    .Respond(HttpStatusCode.BadRequest);
            }

            var client = handler.ToHttpClient();

            return new CoinMarketCapClient(client, options);
        }
    }
}
