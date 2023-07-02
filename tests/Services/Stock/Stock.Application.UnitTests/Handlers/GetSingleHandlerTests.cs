using AutoFixture;
using Cache.Abstract.Contracts;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.Common.Models;
using Stock.Application.Features;
using Stock.Core.Exceptions;
using Tests.Common.FixtureUtilities;

namespace Stock.Application.UnitTests.Handlers
{
    public class GetSingleHandlerTests
    {
        private readonly Fixture _fixture = FixtureHelper.FixtureCircularBehavior();
        private readonly Mock<ICacheService> _cacheMock = new Mock<ICacheService>();
        private readonly ILogger<GetSingle.Handler> _logger = Mock.Of<ILogger<GetSingle.Handler>>();
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
            var query = new GetSingle.Query(symbol);
            _cacheMock.Setup(x => x.Get<StockCacheItem>(It.IsAny<string>()))
                    .ReturnsAsync(_fixture.Build<StockCacheItem>().With(x => x.Symbol, symbol).Create());
            var handler = new GetSingle.Handler(_cacheMock.Object, _localeMock.Object, _logger);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Symbol.Should().Be(symbol);
        }

        
        [Fact]
        public async Task Handle_ShouldThrowException_WhenSymbolDoesntExists()
        {
            var query = new GetSingle.Query("HELLO");
            _cacheMock.Setup(x => x.Get<StockCacheItem>(It.IsAny<string>()))
                .ReturnsAsync((StockCacheItem)null!);
            var handler = new GetSingle.Handler(_cacheMock.Object, _localeMock.Object, _logger);

            await Assert.ThrowsAsync<StockCoreNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        } 
    }
}
