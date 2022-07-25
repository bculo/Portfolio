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
        private const string BTC_SYMBOL = "btc";

        private const string INVALID_SYMBOL = "DRAGON123";

        private static string[] VALID_SYMBOLS = new string[] { BTC_SYMBOL };

        [Fact]
        public async Task FetchData_ShouldReturnInstance_WhenValidSymbolProvided()
        {
            //Arrange
            string symbol = BTC_SYMBOL;
            var service = Build(symbol);

            //Act
            var response = await service.FetchData(symbol);

            //Assert
            Assert.NotNull(response);
            var itemDictionary = response.Data;
            var dictionaryKey = itemDictionary.ContainsKey(symbol) ? symbol : symbol.ToUpper();
            var finalItemsCollection = itemDictionary[dictionaryKey];
            Assert.Contains(finalItemsCollection, i => i.Symbol.ToLower() == symbol.ToLower());
        }

        [Fact]
        public async Task FetchData_ShouldReturnNull_WhenInvalidSymbolProvided()
        {
            //Arrange
            string symbol = INVALID_SYMBOL;
            var service = Build(symbol);

            //Act
            var response = await service.FetchData(symbol);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task FetchData_ShouldReturnNull_WhenRequestUnauthorized()
        {
            //Arrange
            string symbol = BTC_SYMBOL;
            var service = Build(symbol, false);

            //Act
            var response = await service.FetchData(symbol);

            //Assert
            Assert.Null(response);
        }

        public ICryptoInfoService Build(string symbol, bool isAuthorized = true)
        {
            var infoOptions = new CryptoInfoApiOptions
            {
                ApiKey = "<COINMARKETCAP_KEY>",
                BaseUrl = "https://pro-api.coinmarketcap.com/v2/cryptocurrency/",
                HeaderKey = "X-CMC_PRO_API_KEY"
            };

            var options = Options.Create(infoOptions);

            var handler = new MockHttpMessageHandler();

            if(VALID_SYMBOLS.Contains(symbol) && isAuthorized)
            {
                string btcResponseJson = File.ReadAllText("./Static/btc-info-response.json");
                handler.When("https://pro-api.coinmarketcap.com/v2/cryptocurrency/info?symbol=btc")
                    .Respond("application/json", btcResponseJson);
            }
            else if(!VALID_SYMBOLS.Contains(symbol) && isAuthorized)
            {
                handler.When("https://pro-api.coinmarketcap.com/v2/cryptocurrency/*")
                    .Respond(HttpStatusCode.BadRequest);
            }
            else
            {
                handler.When("https://pro-api.coinmarketcap.com/v2/cryptocurrency/*")
                    .Respond(HttpStatusCode.Unauthorized);
            }

            var client = handler.ToHttpClient();

            return new CoinMarketCapClient(client, options);
        }
    }
}
