using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Infrastracture.Persistence;
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

    public class UpdateCryptoItemsPriceConsumerDefinition : ConsumerDefinition<UpdateCryptoItemsPriceConsumer>
    {
        private readonly IServiceProvider _provider;

        public UpdateCryptoItemsPriceConsumerDefinition(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UpdateCryptoItemsPriceConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(config => config.Interval(3, 1000));
            endpointConfigurator.UseEntityFrameworkOutbox<CryptoDbContext>(_provider);
        }
    }
}
