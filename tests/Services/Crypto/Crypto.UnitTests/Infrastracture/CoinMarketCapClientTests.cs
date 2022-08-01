using AutoFixture;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Options;
using Crypto.Infrastracture.Clients;
using Crypto.Mock.Common.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Net;

namespace Crypto.UnitTests.Infrastracture
{
    public class CoinMarketCapClientTests
    {
        private const string BTC_SYMBOL = "BTC";
        private const string INVALID_SYMBOL = "DRAGON123";

        private readonly Fixture _fixture = new Fixture();
        private readonly IOptions<CryptoInfoApiOptions> _options;
        private readonly CryptoDataManager _seeder = new CryptoDataManager();
        private readonly List<string> SUPPORTED_SYMBOLS = new List<string> { BTC_SYMBOL };

        public CoinMarketCapClientTests()
        {
            var infoOptions = new CryptoInfoApiOptions
            {
                ApiKey = "<COINMARKETCAP_KEY>",
                BaseUrl = "https://pro-api.coinmarketcap.com/v2/cryptocurrency/",
                HeaderKey = "X-CMC_PRO_API_KEY"
            };

            _options = Options.Create(infoOptions);

            _seeder.InitData(null, SUPPORTED_SYMBOLS);
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
            var finalItemsCollection = itemDictionary[BTC_SYMBOL];
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

        [Fact]
        public async Task FetchData_ShouldThrowException_WhenTimeoutOccurs()
        {
            //Arrange
            string symbol = BTC_SYMBOL;
            var service = Build(symbol, timeOutException: true);

            await Assert.ThrowsAsync<TaskCanceledException>(() => service.FetchData(symbol));
        }

        public ICryptoInfoService Build(string symbol, bool isAuthorized = true, bool timeOutException = false)
        {
            var handler = new MockHttpMessageHandler();

            if(_seeder.IsSymbolSupported(symbol) && isAuthorized && !timeOutException) //Valid reqeust
            {
                var response = _fixture.Create<CryptoInfoResponseDto>();

                var value = response.Data.First().Value.First();
                value.Symbol = symbol;
                response.Data.Clear();
                response.Data.Add(symbol, new List<CryptoInfoDataDto> { value });

                handler.When("*").Respond("application/json", JsonConvert.SerializeObject(response));
            }
            else if(!_seeder.IsSymbolSupported(symbol) && isAuthorized && !timeOutException) //Invalid symbol
            {
                handler.When("*").Respond(HttpStatusCode.BadRequest);
            }
            else if(!isAuthorized) //401
            {
                handler.When("*").Respond(HttpStatusCode.Unauthorized);
            }
            else if(timeOutException) //Task canceled exception (Timeout occurred)
            {
                handler.When("*").Throw(new TaskCanceledException());
            }
            else
            {
                handler.When("*").Respond(HttpStatusCode.BadRequest);
            }

            return new CoinMarketCapClient(handler.ToHttpClient(), _options);
        }
    }
}
