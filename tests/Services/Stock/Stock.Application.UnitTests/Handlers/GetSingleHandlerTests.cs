using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.Features;
using Stock.Application.Interfaces;
using Stock.Core.Exceptions;
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
        private readonly Mock<IBaseRepository<StockEntity>> _repoMock = new Mock<IBaseRepository<StockEntity>>();

        /*
        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task Handle_ShouldReturnInstance_WhenSymbolExists(string symbol)
        {
            var query = new GetSingle.Query { Symbol = symbol };
            _repoMock.Setup(x => x.First(It.IsAny<Expression<Func<StockEntity, bool>>>()))
                .ReturnsAsync(_fixture.Build<StockEntity>().With(x => x.Symbol, symbol).Create());
            var handler = new GetSingle.Handler(_repoMock.Object);

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
            var handler = new GetSingle.Handler(_repoMock.Object);

            await Assert.ThrowsAsync<StockCoreNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }     
        */
    }
}
