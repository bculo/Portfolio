using AutoFixture;
using Crypto.Mock.Common.Clients;
using Crypto.Mock.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.UnitTests.MockImplementations
{
    public class MockCryptoInfoServiceTests
    {
        private const string BTC_SYMBOL = "BTC";
        private const string INVALID_SYMBOL = "DRAGON123";

        private readonly Fixture _fixture = new Fixture();
        private readonly CryptoDataManager _seeder = new CryptoDataManager();

        public MockCryptoInfoServiceTests()
        {
            _seeder.InitData(null, new List<string> { BTC_SYMBOL });
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

        public MockCryptoInfoService Build(string symbol, bool isAuthorized = true, bool timeOutException = false)
        {
            return new MockCryptoInfoService(_seeder.GetSupportedCryptoSymbolsArray(), isAuthorized, timeOutException);
        }
    }
}
