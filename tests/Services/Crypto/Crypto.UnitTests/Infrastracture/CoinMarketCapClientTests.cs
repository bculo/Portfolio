using AutoFixture;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Options;
using Crypto.Infrastracture.Clients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Net;

namespace Crypto.UnitTests.Infrastracture
{
    public class CoinMarketCapClientTests
    {
        private const string BTC_SYMBOL = "btc";
        private const string INVALID_SYMBOL = "DRAGON123";
        private static string[] VALID_SYMBOLS = new string[] { BTC_SYMBOL };

        private readonly Fixture _fixture = new Fixture();
        private readonly IOptions<CryptoInfoApiOptions> _options;

        public CoinMarketCapClientTests()
        {
            var infoOptions = new CryptoInfoApiOptions
            {
                ApiKey = "<COINMARKETCAP_KEY>",
                BaseUrl = "https://pro-api.coinmarketcap.com/v2/cryptocurrency/",
                HeaderKey = "X-CMC_PRO_API_KEY"
            };

            _options = Options.Create(infoOptions);
        }

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
            var handler = new MockHttpMessageHandler();

            if(VALID_SYMBOLS.Contains(symbol) && isAuthorized)
            {
                var response = _fixture.Create<CryptoInfoResponseDto>();

                var value = response.Data.First().Value.First();
                value.Symbol = symbol;
                response.Data.Clear();
                response.Data.Add(symbol, new List<CryptoInfoDataDto> { value });

                handler.When("*").Respond("application/json", JsonConvert.SerializeObject(response));
            }
            else if(!VALID_SYMBOLS.Contains(symbol) && isAuthorized)
            {
                handler.When("*").Respond(HttpStatusCode.BadRequest);
            }
            else
            {
                handler.When("*").Respond(HttpStatusCode.Unauthorized);
            }

            return new CoinMarketCapClient(handler.ToHttpClient(), _options);
        }
    }
}
