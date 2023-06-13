using AutoFixture;
using FluentAssertions;
using Moq;
using Stock.Application.Features;
using Stock.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.FixtureUtilities;
using Time.Abstract.Contracts;
using StockEntity = Stock.Core.Entities.Stock;

namespace Stock.Application.UnitTests.Handlers
{
    public class GetAllHandlerTests
    {
        private readonly Fixture _fixture = FixtureHelper.FixtureCircularBehavior();
        private readonly Mock<IBaseRepository<StockEntity>> _repoMock = new Mock<IBaseRepository<StockEntity>>();

        [Fact]
        public async Task Handle_ShouldReturnListOfItems_WhenItemsExistsInStorage()
        {
            int numberOfInstances = 5;
            var items = MockInstances(numberOfInstances);
            _repoMock.Setup(x => x.GetAll()).ReturnsAsync(items);
            var query = new GetAll.Query();
            var handler = new GetAll.Handler(_repoMock.Object);

            var handlerResult = await handler.Handle(query, CancellationToken.None);

            handlerResult.Should().NotBeNull().And.HaveCount(numberOfInstances);
        }

        private List<StockEntity> MockInstances(int numberOfInstances)
        {
            return _fixture.CreateMany<StockEntity>(numberOfInstances).ToList();
        }
    }
}
