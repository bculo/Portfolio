﻿using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Consumers.Crypto
{
    public class AddCryptoItemFailedConsumer : IConsumer<AddItemFailed>
    {
        private readonly ILogger<AddCryptoItemFailedConsumer> _logger;

        public AddCryptoItemFailedConsumer(ILogger<AddCryptoItemFailedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<AddItemFailed> context)
        {
            return Task.CompletedTask;
        }
    }
}
