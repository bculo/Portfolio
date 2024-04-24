using Events.Common.Stock;
using MassTransit;
using MediatR;
using Stock.Application.Commands.Stock;

namespace Stock.Infrastructure.Consumers
{
    public class UpdateBatchPreparedConsumer : IConsumer<UpdateBatchPrepared>
    {
        private readonly IMediator _mediator;

        public UpdateBatchPreparedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UpdateBatchPrepared> context)
        {
            await _mediator.Send(new UpdateStockBatch(context.Message.Symbols));
        }
    }
}