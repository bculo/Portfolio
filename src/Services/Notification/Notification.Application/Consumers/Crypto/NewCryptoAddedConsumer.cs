using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Mediator;

namespace Notification.Application.Consumers.Crypto
{
    public class NewCryptoAddedConsumer : IConsumer<NewItemAdded>
    {
        private readonly IMediator _mediator;

        public NewCryptoAddedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<NewItemAdded> context)
        {
            var instance = context.Message;
            var command = new Commands.Crypto.SendPriceUpdatedNotification(instance.Symbol, instance.Price);
            await _mediator.Send(command);
        }
    }
}
