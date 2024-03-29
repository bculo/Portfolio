﻿using Events.Common.Stock;
using MassTransit;
using MediatR;
using Stock.Application.Commands.Cache;

namespace Stock.Infrastructure.Consumers
{
    public class PriceUpdatedConsumer : IConsumer<PriceUpdated>
    {
        private readonly IMediator _mediator;
        
        public PriceUpdatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PriceUpdated> context)
        {
            var body = context.Message;
            await _mediator.Send(new RefreshStockItemValue(body.Id, body.Symbol, body.Price));
        }
    }
}
