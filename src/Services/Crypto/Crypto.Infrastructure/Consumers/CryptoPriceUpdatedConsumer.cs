using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Crypto.Infrastructure.Consumers
{
    public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
    {
        private readonly ILogger<CryptoPriceUpdatedConsumer> _logger;

        public CryptoPriceUpdatedConsumer(ILogger<CryptoPriceUpdatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
        {

        }
    }
}
