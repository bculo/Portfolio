﻿using Events.Common.Stock;
using MassTransit;
using MediatR;
using Stock.Application.Commands.Stock;

namespace Stock.Infrastructure.Consumers
{
    public class BatchForUpdatePreparedConsumer : IConsumer<BatchForUpdatePrepared>
    {
        private readonly IMediator _mediator;

        public BatchForUpdatePreparedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BatchForUpdatePrepared> context)
        {
            await _mediator.Send(new UpdateStockBatch(context.Message.Symbols));
        }
    }
}
