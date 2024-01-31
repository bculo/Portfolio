using MediatR;
using Stock.Application.Commands.Stock;

namespace Stock.Worker.Jobs
{
    public interface ICreateBatchJob
    {
        Task InitializeUpdateProcedure();
    }

    public class CreateBatchJob : ICreateBatchJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateBatchJob> _logger;

        public CreateBatchJob(IMediator mediator,
            ILogger<CreateBatchJob> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task InitializeUpdateProcedure()
        {
            _logger.LogTrace("PrepareBatchesForUpdate.Command");
            await _mediator.Send(new CreateStockUpdateBatches(), default);
        }
    }
}
