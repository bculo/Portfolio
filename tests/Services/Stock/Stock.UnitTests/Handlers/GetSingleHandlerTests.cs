using FluentAssertions;
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
    public class GetSingleHandlerTests
    {
        private readonly Mock<StockDbContext> _dbContextMock;
        private readonly ILogger<GetSingle.Handler> _logger = Mock.Of<ILogger<GetSingle.Handler>>();

        public GetSingleHandlerTests()
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
        public async Task Handle_ShouldReturnInstance_WhenSymbolExists(string symbol)
        {
            var query = new GetSingle.Query { Symbol = symbol };

            var items = new List<Core.Entities.Stock>
            {
                new Core.Entities.Stock
                {
                    Id = 1,
                    Symbol = "RANDOM",
                },
                new Core.Entities.Stock
                {
                    Id = 2,
                    Symbol = symbol,
                }
            };

            _dbContextMock.Setup(x => x.Stocks).ReturnsDbSet(items);
            var handler = new GetSingle.Handler(_logger, _dbContextMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Symbol.Should().Be(symbol);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSymbolDoesntExists()
        {
            var query = new GetSingle.Query { Symbol = "HELLO" };

            var items = new List<Core.Entities.Stock>
            {
                new Core.Entities.Stock
                {
                    Id = 1,
                    Symbol = "RANDOM",
                },
                new Core.Entities.Stock
                {
                    Id = 2,
                    Symbol = "TSLA",
                }
            };

            _dbContextMock.Setup(x => x.Stocks).ReturnsDbSet(items);
            var handler = new GetSingle.Handler(_logger, _dbContextMock.Object);

            await  Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
