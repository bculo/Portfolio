using AutoFixture;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Stock.Application.Features;
using Stock.Application.Infrastructure.Persistence;
using Stock.Application.Interfaces;
using Time.Abstract.Contracts;

namespace Stock.UnitTests.Handlers
{
    public class AddNewHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<StockDbContext> _dbContextMock;
        private readonly IPublishEndpoint _publishEndpoint = Mock.Of<IPublishEndpoint>();
        private readonly ILogger<AddNew.Handler> _logger = Mock.Of<ILogger<AddNew.Handler>>();
        private readonly Mock<IStockPriceClient> _priceClientMock = new Mock<IStockPriceClient>();

        public AddNewHandlerTests()
        {
            var configuration = new Mock<IConfiguration>();
            var timeProviderMock = new Mock<IDateTimeProvider>();
            timeProviderMock.Setup(s => s.Now).Returns(DateTime.UtcNow);
            var currentUserMock = new Mock<IStockUser>();
            currentUserMock.Setup(s => s.Identifier).Returns(Guid.NewGuid());

            _dbContextMock = new Mock<StockDbContext>(configuration.Object, timeProviderMock.Object, currentUserMock.Object);
        }

        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task Handle_ShouldThrowException_WhenExistingSymbolProvided(string symbol)
        {
            var items = MockListOfItems(symbol);
            _dbContextMock.Setup(m => m.Stocks).ReturnsDbSet(items);
            var handler = new AddNew.Handler(_logger, _dbContextMock.Object, _priceClientMock.Object, _publishEndpoint);
            var command = new AddNew.Command { Symbol = symbol };

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData("RAND")]
        [InlineData("BAND")]
        public async Task Handle_ShouldReturnInstanceIdentifier_WhenNewSupportedSymbolProvided(string symbol)
        {
            var items = MockListOfItems();
            _dbContextMock.Setup(m => m.Stocks).ReturnsDbSet(items);
            _priceClientMock.Setup(m => m.GetPriceForSymbol(It.IsAny<string>()))
                .Returns(Task.FromResult(
                    _fixture.Build<StockPriceInfo>().With(i => i.Symbol, symbol).Create()));
            var handler = new AddNew.Handler(_logger, _dbContextMock.Object, _priceClientMock.Object, _publishEndpoint);
            var command = new AddNew.Command { Symbol = symbol };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeOfType(typeof(long));
        }

        [Theory]
        [InlineData("TOP")]
        [InlineData("KEK")]
        public async Task Handle_ShouldThrowException_WhenNewUnsupportedSymbolProvided(string symbol)
        {
            var items = MockListOfItems();
            _dbContextMock.Setup(m => m.Stocks).ReturnsDbSet(items);
            _priceClientMock.Setup(m => m.GetPriceForSymbol(It.IsAny<string>()))
                .Returns(Task.FromResult(null as StockPriceInfo));
            var handler = new AddNew.Handler(_logger, _dbContextMock.Object, _priceClientMock.Object, _publishEndpoint);
            var command = new AddNew.Command { Symbol = symbol };

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        private IEnumerable<Core.Entities.Stock> MockListOfItems(string symbolThatWillBeInList)
        {
            var symbols = _fixture.CreateMany<string>(10).ToList();
            symbols[2] = symbolThatWillBeInList;
            return symbols.Select((i, index) => new Core.Entities.Stock { Id = index + 1, Symbol = i });
        }

        private IEnumerable<Core.Entities.Stock> MockListOfItems()
        {
            var symbols = _fixture.CreateMany<string>(10).ToList();
            return symbols.Select((i, index) => new Core.Entities.Stock { Id = index + 1, Symbol = i });
        }
    }
}
