using MediatR;
using Stock.Application.Commands.Stock;

namespace Stock.Worker.Jobs
{
    public interface IPriceUpdateJobService
    {
        Task InitializeUpdateProcedure();
    }

    public class UpdateStockPriceHangfireJob : IPriceUpdateJobService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateStockPriceHangfireJob> _logger;

        public UpdateStockPriceHangfireJob(IMediator mediator,
            ILogger<UpdateStockPriceHangfireJob> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task InitializeUpdateProcedure()
        {
            _logger.LogTrace("PrepareBatchesForUpdate.Command");
            await _mediator.Send(new CreateStockUpdateBatches(), CancellationToken.None);
        }
    }
}
