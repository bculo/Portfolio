using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Modules.EventHandlers.Crypto;

namespace Notification.Application.Consumers.Crypto
{
    public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CryptoPriceUpdatedConsumer> _logger;

        public CryptoPriceUpdatedConsumer(IMediator mediator,
            ILogger<CryptoPriceUpdatedConsumer> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
        {
            var instance = context.Message;

            _logger.LogTrace($"Crypto updated. {instance.Symbol} - {instance.Price} {instance.Currency}");

            await _mediator.Publish(new CryptoPriceUpdatedHandler.Notification
            {
                Currency = instance.Currency,
                Price = instance.Price,
                Id = instance.Id,
                Name = instance.Name,
                Symbol = instance.Symbol,
            });
        }
    }
}
