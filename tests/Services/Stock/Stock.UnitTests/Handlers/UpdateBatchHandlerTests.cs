using AutoFixture;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using Stock.Application.Features;
using Stock.Application.Infrastructure.Persistence;
using Stock.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Abstract.Contracts;

namespace Stock.UnitTests.Handlers
{
    public class UpdateBatchHandlerTests
    {
        private readonly Fixture _fxiture = new Fixture();
        private readonly Mock<StockDbContext> _dbContextMock;
        private readonly IPublishEndpoint _endpoint = Mock.Of<IPublishEndpoint>();
        private readonly Mock<IDateTimeProvider> _timeMock = new Mock<IDateTimeProvider>();
        private readonly Mock<IStockPriceClient> _clientMock = new Mock<IStockPriceClient>();
        private readonly ILogger<UpdateBatch.Handler> _logger = Mock.Of<ILogger<UpdateBatch.Handler>>();

        public UpdateBatchHandlerTests()
        {
            _timeMock.Setup(x => x.Now).Returns(DateTime.Now);

            var config = Mock.Of<IConfiguration>();
            var userMock = new Mock<IStockUser>();
            userMock.Setup(x => x.Identifier).Returns(Guid.NewGuid());

            _dbContextMock = new Mock<StockDbContext>(config, _timeMock.Object, userMock.Object);
        }

        [Fact]
        public void Test()
        {
            _clientMock.Setup(x => x.GetPriceForSymbol(It.IsAny<string>())
                .Returns(_fxiture.Create<StockPriceInfo>()));

        }
    }
}
