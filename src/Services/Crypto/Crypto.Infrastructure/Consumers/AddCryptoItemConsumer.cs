using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Infrastructure.Consumers
{
    public class AddCryptoItemConsumer : IConsumer<AddItem>
    {
        private readonly IMediator _mediator;

        public AddCryptoItemConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<AddItem> context)
        {
            await _mediator.Send(new AddNewCommand
            {
                Symbol = context.Message.Symbol,
                CorrelationId = context.CorrelationId
            });
        }
    }
}
