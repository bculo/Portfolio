using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using Stock.Application.Features;
using Stock.Application.Infrastructure.Persistence;
using Stock.Application.Interfaces;
using Time.Abstract.Contracts;

namespace Stock.UnitTests.Handlers
{
    public class GetAllHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<StockDbContext> _dbContextMock;

        public GetAllHandlerTests()
        {
            var configuration = new Mock<IConfiguration>();
            var timeProviderMock = new Mock<IDateTimeProvider>();
            timeProviderMock.Setup(s => s.Now).Returns(DateTime.UtcNow);
            var currentUserMock = new Mock<IStockUser>();
            currentUserMock.Setup(s => s.Identifier).Returns(Guid.NewGuid());

            _dbContextMock = new Mock<StockDbContext>(configuration.Object, timeProviderMock.Object, currentUserMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfItems_WhenItemsExistsInStorage()
        {
            int numberOfInstances = 5;
            var items = MockInstances(numberOfInstances);
            _dbContextMock.Setup(x => x.Stocks).ReturnsDbSet(items);
            var query = new GetAll.Query();
            var handler = new GetAll.Handler(_dbContextMock.Object);

            var handlerResult = await handler.Handle(query, CancellationToken.None);

            handlerResult.Should().NotBeNull().And.HaveCount(numberOfInstances);
        }

        private IEnumerable<Core.Entities.Stock> MockInstances(int numberOfInstances)
        {
            return _fixture.CreateMany<Core.Entities.Stock>(numberOfInstances);
        }
    }
}
