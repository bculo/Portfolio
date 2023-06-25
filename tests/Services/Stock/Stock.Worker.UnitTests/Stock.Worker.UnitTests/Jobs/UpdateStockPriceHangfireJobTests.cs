using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.Features;
using Stock.Worker.Jobs;

namespace Stock.Worker.UnitTests.Jobs
{
    public class UpdateStockPriceHangfireJobTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly ILogger<UpdateStockPriceHangfireJob> _logger = Mock.Of<ILogger<UpdateStockPriceHangfireJob>>();

        public UpdateStockPriceHangfireJobTests()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<PrepareBatchesForUpdate.Command>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task InitializeUpdateProcedure_ShouldNotThrowException_WhenExecuted()
        {
            var job = new UpdateStockPriceHangfireJob(_mediatorMock.Object, _logger);

            var exception = await Record.ExceptionAsync(job.InitializeUpdateProcedure);

            exception.Should().BeNull();
            _mediatorMock.Verify(x => x.Send(It.IsAny<PrepareBatchesForUpdate.Command>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
