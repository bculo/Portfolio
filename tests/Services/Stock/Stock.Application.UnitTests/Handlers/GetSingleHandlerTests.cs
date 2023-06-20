using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.Features;
using Stock.Application.Interfaces;
using Stock.Core.Exceptions;
using Stock.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.FixtureUtilities;
using Time.Abstract.Contracts;
using StockEntity = Stock.Core.Entities.Stock;

namespace Stock.Application.UnitTests.Handlers
{
    public class GetSingleHandlerTests
    {
        private readonly Fixture _fixture = FixtureHelper.FixtureCircularBehavior();
        private readonly Mock<IStockRepository> _repoMock = new Mock<IStockRepository>();
        private readonly Mock<IStringLocalizer<GetSingleLocale>> _localeMock = new Mock<IStringLocalizer<GetSingleLocale>>();

        public GetSingleHandlerTests()
        {
            _localeMock.Setup(x => x[It.IsAny<string>()])
                .Returns((string passedValue) => new LocalizedString(passedValue, passedValue));
        }

        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task Handle_ShouldReturnInstance_WhenSymbolExists(string symbol)
        {
            var query = new GetSingle.Query { Symbol = symbol };
            _repoMock.Setup(x => x.GetCurrentPrice(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Build<StockPriceTagQuery>().With(x => x.Symbol, symbol).Create());
            var handler = new GetSingle.Handler(_repoMock.Object, _localeMock.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Symbol.Should().Be(symbol);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSymbolDoesntExists()
        {
            var query = new GetSingle.Query { Symbol = "HELLO" };
            _repoMock.Setup(x => x.First(It.IsAny<Expression<Func<StockEntity, bool>>>()))
                .ReturnsAsync((StockEntity)null!);
            var handler = new GetSingle.Handler(_repoMock.Object, _localeMock.Object);

            await Assert.ThrowsAsync<StockCoreNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }     
    }
}
