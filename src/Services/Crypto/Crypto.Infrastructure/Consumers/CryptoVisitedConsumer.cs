﻿using Crypto.Application.Modules.Crypto.Commands.Visited;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastructure.Consumers
{
    public class CryptoVisitedConsumer : IConsumer<Visited>
    {
        private readonly ILogger<CryptoVisitedConsumer> _logger;
        private readonly IMediator _mediator;

        public CryptoVisitedConsumer(ILogger<CryptoVisitedConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<Visited> context)
        {
            await _mediator.Send(new VisitedCommand
            {
                Symbol = context.Message.Symbol, 
                CryptoId = context.Message.CryptoId
            });
        }
    }
}
