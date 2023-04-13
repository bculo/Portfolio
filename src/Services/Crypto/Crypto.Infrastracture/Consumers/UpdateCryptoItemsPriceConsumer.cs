using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Infrastracture.Consumers
{
    public class UpdateCryptoItemsPriceConsumer : IConsumer<UpdateCryptoItemsPrice>
    {
        private readonly IMediator _mediator;

        public UpdateCryptoItemsPriceConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UpdateCryptoItemsPrice> context)
        {
            await _mediator.Send(new UpdatePriceAllCommand { });
        }
    }
}
