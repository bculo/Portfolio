using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Application.Consumers
{
    public class AddCryptoItemConsumer : IConsumer<AddCryptoItem>
    {
        private readonly IMediator _mediator;

        public AddCryptoItemConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<AddCryptoItem> context)
        {
            await _mediator.Send(new AddNewCommand
            {
                Symbol = context.Message.Symbol,
                TemporaryId = context.Message.TemporaryId
            });
        }
    }
}
