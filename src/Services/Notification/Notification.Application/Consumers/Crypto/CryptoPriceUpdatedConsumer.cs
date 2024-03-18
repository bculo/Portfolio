using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Notification.Application.Consumers.Crypto
{
    public class CryptoPriceUpdatedConsumer : IConsumer<PriceUpdated>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CryptoPriceUpdatedConsumer> _logger;

        public CryptoPriceUpdatedConsumer(IMediator mediator,
            ILogger<CryptoPriceUpdatedConsumer> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PriceUpdated> context)
        {
            var instance = context.Message;

            _logger.LogTrace($"Crypto updated. {instance.Symbol} - {instance.Price}");
        }
    }
}
