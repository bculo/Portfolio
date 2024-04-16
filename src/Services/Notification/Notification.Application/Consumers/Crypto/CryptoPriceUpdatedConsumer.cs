﻿using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Notification.Application.Consumers.Crypto
{
    public class CryptoPriceUpdatedConsumer : IConsumer<PriceUpdated>
    {
        private readonly IMediator _mediator;

        public CryptoPriceUpdatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PriceUpdated> context)
        {
            var instance = context.Message;
            var command = new Commands.Crypto.SendPriceUpdatedNotification(instance.Symbol, instance.Price);
            await _mediator.Send(command);
        }
    }
}
