using Crypto.Application.Common.Options;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;

namespace Crypto.Infrastructure.Consumers
{
    public class AddCryptoItemConsumer(IMediator mediator) : IConsumer<AddItem>
    {
        public async Task Consume(ConsumeContext<AddItem> context)
        {
            await mediator.Send(new AddNewCommand
            {
                Symbol = context.Message.Symbol,
                CorrelationId = context.CorrelationId
            });
        }
    }
}
