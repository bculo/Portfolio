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
    public class NewCryptoAddedConsumer : IConsumer<NewItemAdded>
    {
        private readonly ILogger<NewCryptoAddedConsumer> _logger;

        public NewCryptoAddedConsumer(ILogger<NewCryptoAddedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<NewItemAdded> context)
        {
            return Task.CompletedTask;
        }
    }
}
