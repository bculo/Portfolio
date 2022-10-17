using Crypto.Application.Interfaces.Services;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Consumers
{
    public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
    {
        private readonly ILogger<CryptoPriceUpdatedConsumer> _logger;
        private readonly INotificationService _notification;

        public CryptoPriceUpdatedConsumer(ILogger<CryptoPriceUpdatedConsumer> logger,
            INotificationService notification)
        {
            _logger = logger;
            _notification = notification;
        }

        public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
        {
            _logger.LogTrace("New message received {0}", JsonConvert.SerializeObject(context.Message));

            await _notification.NotifyAboutPriceUpdate(context.Message);
        }
    }
}
