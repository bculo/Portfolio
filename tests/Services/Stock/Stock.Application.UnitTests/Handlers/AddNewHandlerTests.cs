using AutoFixture;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Localization;
using Moq;
using Stock.Application.Features;
using Stock.Application.Interfaces;
using Stock.Core.Exceptions;
using System.Linq.Expressions;
using StockEntity = Stock.Core.Entities.Stock;


namespace Stock.Application.UnitTests.Handlers
{
    public class AddNewHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly IPublishEndpoint _publishEndpoint = Mock.Of<IPublishEndpoint>();
        private readonly Mock<IStockPriceClient> _priceClientMock = new Mock<IStockPriceClient>();
        private readonly Mock<IBaseRepository<StockEntity>> _repoMock = new Mock<IBaseRepository<StockEntity>>();
        private readonly Mock<IStringLocalizer<AddNewLocale>> _localeMock = new Mock<IStringLocalizer<AddNewLocale>>();

        public AddNewHandlerTests()
        {
            _localeMock.Setup(x => x[It.IsAny<string>()])
                .Returns((string passedValue) => new LocalizedString(passedValue, passedValue));
        }

        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task Handle_ShouldThrowException_WhenExistingSymbolProvided(string symbol)
        {
            var instance = new StockEntity { Symbol = symbol };
            _repoMock.Setup(m => m.First(It.IsAny<Expression<Func<StockEntity, bool>>>())).ReturnsAsync(instance);
            var handler = new AddNew.Handler(_priceClientMock.Object, _publishEndpoint, _repoMock.Object, _localeMock.Object);
            var command = new AddNew.Command { Symbol = symbol };

            await Assert.ThrowsAsync<StockCoreException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData("RAND")]
        [InlineData("BAND")]
        public async Task Handle_ShouldReturnInstanceIdentifier_WhenNewSupportedSymbolProvided(string symbol)
        {
            var instance = new StockEntity { Symbol = symbol };
            _repoMock.Setup(m => m.First(It.IsAny<Expression<Func<StockEntity, bool>>>())).ReturnsAsync((StockEntity)default!);
            _priceClientMock.Setup(m => m.GetPriceForSymbol(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Build<StockPriceInfo>().With(i => i.Symbol, symbol).Create());
            var handler = new AddNew.Handler(_priceClientMock.Object, _publishEndpoint, _repoMock.Object, _localeMock.Object);
            var command = new AddNew.Command { Symbol = symbol };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeOfType(typeof(long));
        }

        [Theory]
        [InlineData("TOP")]
        [InlineData("KEK")]
        public async Task Handle_ShouldThrowException_WhenNewUnsupportedSymbolProvided(string symbol)
        {
            var instance = new StockEntity { Symbol = symbol };
            _repoMock.Setup(m => m.First(It.IsAny<Expression<Func<StockEntity, bool>>>())).ReturnsAsync((StockEntity)default!);
            _priceClientMock.Setup(m => m.GetPriceForSymbol(It.IsAny<string>()))
                .ReturnsAsync(null as StockPriceInfo);
            var handler = new AddNew.Handler(_priceClientMock.Object, _publishEndpoint, _repoMock.Object, _localeMock.Object);
            var command = new AddNew.Command { Symbol = symbol };

            await Assert.ThrowsAsync<StockCoreException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
