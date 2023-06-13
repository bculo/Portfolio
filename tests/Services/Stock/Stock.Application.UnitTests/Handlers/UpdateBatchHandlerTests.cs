using AutoFixture;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.Features;
using Stock.Application.Interfaces;
using Stock.Core.Entities;
using System.Linq.Expressions;
using Tests.Common.FixtureUtilities;
using Time.Abstract.Contracts;
using StockEntity = Stock.Core.Entities.Stock;

namespace Stock.Application.UnitTests.Handlers
{
    public class UpdateBatchHandlerTests
    {
        private readonly Fixture _fixture = FixtureHelper.FixtureCircularBehavior();
        private readonly IPublishEndpoint _endpoint = Mock.Of<IPublishEndpoint>();
        private readonly Mock<IDateTimeProvider> _timeMock = new Mock<IDateTimeProvider>();
        private readonly Mock<IStockPriceClient> _clientMock = new Mock<IStockPriceClient>();
        private readonly ILogger<UpdateBatch.Handler> _logger = Mock.Of<ILogger<UpdateBatch.Handler>>();
        private readonly Mock<IBaseRepository<StockEntity>> _repoStockMock = new Mock<IBaseRepository<StockEntity>>();
        private readonly Mock<IBaseRepository<StockPrice>> _repoStockPriceMock = new Mock<IBaseRepository<StockPrice>>();

        public UpdateBatchHandlerTests()
        {
            _timeMock.Setup(x => x.Now).Returns(DateTime.Now);
        }

        [Fact]
        public async Task Handle_ShouldStopExecution_WhenThereAreZeroItemsToUpdate()
        {
            _repoStockMock.Setup(x => x.GetDictionary(It.IsAny<Expression<Func<StockEntity, bool>>>(),
                It.IsAny<Func<StockEntity, string>>(),
                It.IsAny<Func<StockEntity, int>>())
            ).ReturnsAsync(new Dictionary<string, int>());

            var handler = new UpdateBatch.Handler(_clientMock.Object, _logger, _endpoint,
                _timeMock.Object, _repoStockPriceMock.Object, _repoStockMock.Object);

            var command = new UpdateBatch.Command
            {
                Symbols = new List<string> { "AAPL", "TSLA" }
            };

            await handler.Handle(command, CancellationToken.None);

            _repoStockPriceMock.Verify(x => x.SaveChanges(), Times.Never());
        }

        [Fact]
        public async Task Handle_ShouldUpdateItems_WhenItemsExistsInStorageAndSupportedByClient()
        {
            _repoStockMock.Setup(x => x.GetDictionary(It.IsAny<Expression<Func<StockEntity, bool>>>(),
                It.IsAny<Func<StockEntity, string>>(),
                It.IsAny<Func<StockEntity, int>>())
            ).ReturnsAsync(new Dictionary<string, int>()
            {
                { "AAPL", 1 },
                { "TSLA", 2 },
            });

            _clientMock.Setup(x => x.GetPriceForSymbol(It.IsAny<string>()))
                .ReturnsAsync((string passedValue) => _fixture.Build<StockPriceInfo>().With(x => x.Symbol, passedValue).Create());

            var handler = new UpdateBatch.Handler(_clientMock.Object, _logger, _endpoint,
                _timeMock.Object, _repoStockPriceMock.Object, _repoStockMock.Object);

            var command = new UpdateBatch.Command
            {
                Symbols = new List<string> { "AAPL", "TSLA" }
            };

            await handler.Handle(command, CancellationToken.None);

            _repoStockPriceMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public async Task Handle_ShouldUpdateItems_WhenItemsExistsInStorageButNotSupportedByClient()
        {
            _repoStockMock.Setup(x => x.GetDictionary(It.IsAny<Expression<Func<StockEntity, bool>>>(),
                It.IsAny<Func<StockEntity, string>>(),
                It.IsAny<Func<StockEntity, int>>())
            ).ReturnsAsync(new Dictionary<string, int>()
            {
                { "AAPL", 1 },
                { "TSLA", 2 },
            });

            _clientMock.Setup(x => x.GetPriceForSymbol(It.IsAny<string>()))
                .ReturnsAsync((StockPriceInfo)null!);

            var handler = new UpdateBatch.Handler(_clientMock.Object, _logger, _endpoint,
                _timeMock.Object, _repoStockPriceMock.Object, _repoStockMock.Object);

            var command = new UpdateBatch.Command
            {
                Symbols = new List<string> { "AAPL", "TSLA" }
            };

            await handler.Handle(command, CancellationToken.None);

            _repoStockPriceMock.Verify(x => x.SaveChanges(), Times.Never());
        }
    }
}
