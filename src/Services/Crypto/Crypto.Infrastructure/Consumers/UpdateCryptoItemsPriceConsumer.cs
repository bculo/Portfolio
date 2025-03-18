using Crypto.Application.Modules.Crypto.Commands;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Infrastructure.Consumers
{
    public class UpdateCryptoItemsPriceConsumer(IMediator mediator) : IConsumer<UpdateItemsPrices>
    {
        public async Task Consume(ConsumeContext<UpdateItemsPrices> context)
        {
            await mediator.Send(new UpdatePriceAllCommand());
        }
    }
}
