using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Infrastructure.Persistence;
using Events.Common.Crypto;
using Hangfire.Logging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Infrastructure.Consumers
{
    public class UpdateCryptoItemsPriceConsumer(
        IMediator mediator,
        ILogger<UpdateCryptoItemsPriceConsumer> logger)
        : IConsumer<UpdateItemsPrices>
    {
        private readonly ILogger<UpdateCryptoItemsPriceConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<UpdateItemsPrices> context)
        {
            await mediator.Send(new UpdatePriceAllCommand { });
        }
    }
}
