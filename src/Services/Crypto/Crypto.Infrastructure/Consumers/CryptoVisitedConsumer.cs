using Crypto.Application.Modules.Crypto.Commands;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Infrastructure.Consumers
{
    public class CryptoVisitedConsumer(IMediator mediator) : IConsumer<Visited>
    {
        public async Task Consume(ConsumeContext<Visited> context)
        {
            await mediator.Send(new VisitedCommand(context.Message.CryptoId, context.Message.Symbol));
        }
    }
}