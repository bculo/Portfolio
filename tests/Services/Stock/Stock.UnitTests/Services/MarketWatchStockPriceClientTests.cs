using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Stock.Application.Common.Constants;
using Stock.Application.Interfaces;
using Stock.Infrastructure.Clients;
using System.Globalization;
using System.Net;
using Time.Abstract.Contracts;

namespace Stock.UnitTests.Services
{
    public class MarketWatchStockPriceClientTests
    {
        private const string BASE_ADDRESS = "http://localhost/api/";

        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IHtmlParser> _htmlParser = new Mock<IHtmlParser>();
        private readonly MockHttpMessageHandler _httpHandler = new MockHttpMessageHandler();
        private readonly Mock<IDateTimeProvider> _timeProvider = new Mock<IDateTimeProvider>();
        private readonly Mock<IServiceProvider> _serivceProvider = new Mock<IServiceProvider>();
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly ILogger<MarketWatchStockPriceClient> _logger = Mock.Of<ILogger<MarketWatchStockPriceClient>>();
        
        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task GetPriceForSymbol_ShouldReturnValidResponse_WhenInstanceExistsEnCulture(string symbol)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string priceAsString = "1,012.33";

            var responseInstance = JsonConvert.SerializeObject(
                _fixture.Build<StockPriceInfo>()
                    .With(p => p.Symbol, symbol)
                    .Create());

            _httpHandler.When("*").Respond("application/json", responseInstance);
            var httpClient = _httpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri(BASE_ADDRESS);
            _httpClientFactory.Setup(x => x.CreateClient(HttpClientNames.MARKET_WATCH))
                .Returns(httpClient);

            _htmlParser.Setup(x => x.FindFirstElement(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    _fixture.Build<HtmlNodeElement>()
                        .With(p => p.Text, priceAsString)
                        .Create()));

            _serivceProvider.Setup(x => x.GetService(typeof(IHtmlParser))).Returns(_htmlParser.Object);

            var client = new MarketWatchStockPriceClient(
                _serivceProvider.Object, 
                _logger, 
                _httpClientFactory.Object, 
                _timeProvider.Object);

            var response =  await client.GetPriceForSymbol(symbol);

            response.Should().NotBeNull();
            response.Symbol.Should().Be(symbol);
            response.Price.Should().Be(1012.33m);
        }

        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task GetPriceForSymbol_ShouldReturnValidResponse_WhenInstanceExistsCroCulture(string symbol)
        {
            CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
            string priceAsString = "1.012,33";

            var responseInstance = JsonConvert.SerializeObject(
                _fixture.Build<StockPriceInfo>()
                    .With(p => p.Symbol, symbol)
                    .Create());

            _httpHandler.When("*").Respond("application/json", responseInstance);
            var httpClient = _httpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri(BASE_ADDRESS);
            _httpClientFactory.Setup(x => x.CreateClient(HttpClientNames.MARKET_WATCH))
                .Returns(httpClient);

            _htmlParser.Setup(x => x.FindFirstElement(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    _fixture.Build<HtmlNodeElement>()
                        .With(p => p.Text, priceAsString)
                        .Create()));

            _serivceProvider.Setup(x => x.GetService(typeof(IHtmlParser))).Returns(_htmlParser.Object);

            var client = new MarketWatchStockPriceClient(
                _serivceProvider.Object,
                _logger,
                _httpClientFactory.Object,
                _timeProvider.Object);

            var response = await client.GetPriceForSymbol(symbol);

            response.Should().NotBeNull();
            response.Symbol.Should().Be(symbol);
            response.Price.Should().Be(1012.33m);
        }

        [Theory]
        [InlineData("BEP")]
        [InlineData("MEP")]
        public async Task GetPriceForSymbol_ShouldReturnNull_WhenInstanceDoesntExists(string symbol)
        {
            _httpHandler.When("*").Respond(HttpStatusCode.BadRequest);
            var httpClient = _httpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri(BASE_ADDRESS);
            _httpClientFactory.Setup(x => x.CreateClient(HttpClientNames.MARKET_WATCH))
                .Returns(httpClient);

            _serivceProvider.Setup(x => x.GetService(typeof(IHtmlParser))).Returns(_htmlParser.Object);

            var client = new MarketWatchStockPriceClient(
                _serivceProvider.Object,
                _logger,
                _httpClientFactory.Object,
                _timeProvider.Object);

            var response = await client.GetPriceForSymbol(symbol);

            response.Should().BeNull();
        }
    }
}
