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
    public class CryptoItemDeletedConsumer : IConsumer<ItemDeleted>
    {
        private readonly ILogger<CryptoItemDeletedConsumer> _logger;

        public CryptoItemDeletedConsumer(ILogger<CryptoItemDeletedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ItemDeleted> context)
        {
            return Task.CompletedTask;
        }
    }
}
