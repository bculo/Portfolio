using AutoFixture;
using FluentAssertions;
using Moq;
using Stock.Application.Features;
using Stock.Application.Interfaces;
using Stock.Core.Queries;
using Tests.Common.FixtureUtilities;

namespace Stock.Application.UnitTests.Handlers
{
    public class GetAllHandlerTests
    {
        private readonly Fixture _fixture = FixtureHelper.FixtureCircularBehavior();
        private readonly Mock<IStockRepository> _repoMock = new Mock<IStockRepository>();

        [Fact]
        public async Task Handle_ShouldReturnListOfItems_WhenItemsExistsInStorage()
        {
            int numberOfInstances = 5;
            var items = MockInstances(numberOfInstances);
            _repoMock.Setup(x => x.GetAllWithPrice()).ReturnsAsync(items);
            var query = new GetAll.Query();
            var handler = new GetAll.Handler(_repoMock.Object);

            var handlerResult = await handler.Handle(query, CancellationToken.None);

            handlerResult.Should().NotBeNull().And.HaveCount(numberOfInstances);
        }

        private List<StockPriceTagQuery> MockInstances(int numberOfInstances)
        {
            return _fixture.CreateMany<StockPriceTagQuery>(numberOfInstances).ToList();
        }
    }
}
