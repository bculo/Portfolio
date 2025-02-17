using MediatR;
using Stock.Application.Commands.Stock;

namespace Stock.Worker.Jobs
{
    public interface ICreateBatchJob
    {
        Task InitializeUpdateProcedure();
    }

    public class CreateBatchJob(ISender mediator) : ICreateBatchJob
    {
        public async Task InitializeUpdateProcedure()
        {
            await mediator.Send(new PrepareStockUpdateBatches(), default);
        }
    }
}
