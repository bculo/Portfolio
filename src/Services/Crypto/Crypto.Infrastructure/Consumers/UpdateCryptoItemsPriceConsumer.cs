using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Infrastructure.Persistence;
using Events.Common.Crypto;
using Hangfire.Logging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Infrastructure.Consumers
{
    public class UpdateCryptoItemsPriceConsumer : IConsumer<UpdateCryptoPrices>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateCryptoItemsPriceConsumer> _logger;

        public UpdateCryptoItemsPriceConsumer(IMediator mediator, 
            ILogger<UpdateCryptoItemsPriceConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UpdateCryptoPrices> context)
        {
            await _mediator.Send(new UpdatePriceAllCommand { });
        }
    }
}
