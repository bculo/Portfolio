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

        public CreateBatchJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task InitializeUpdateProcedure()
        {
            await _mediator.Send(new CreateStockUpdateBatches(), default);
        }
    }
}
