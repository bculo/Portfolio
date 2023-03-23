using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Consumers.Crypto
{
    public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
    {
        private readonly ILogger<CryptoPriceUpdatedConsumer> _logger;

        public CryptoPriceUpdatedConsumer(ILogger<CryptoPriceUpdatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CryptoPriceUpdated> context)
        {
            var instance = context.Message;

            _logger.LogTrace($"Crypto updated. {instance.Symbol} - {instance.Price} {instance.Currency}");

            return Task.CompletedTask;
        }
    }
}
