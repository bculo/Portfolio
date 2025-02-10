using Events.Common.Stock;
using MassTransit;
using MediatR;
using Stock.Application.Commands.Stock;

namespace Stock.Infrastructure.Consumers
{
    public class UpdateBatchPreparedConsumer(IMediator mediator) : IConsumer<UpdateBatchPrepared>
    {
        public async Task Consume(ConsumeContext<UpdateBatchPrepared> context)
        {
            await mediator.Send(new UpdateStockBatchCommand(context.Message.Symbols));
        }
    }
}