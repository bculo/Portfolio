using Crypto.Application.Modules.Crypto.Commands.Visited;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Infrastructure.Consumers
{
    public class CryptoVisitedConsumer(ILogger<CryptoVisitedConsumer> logger, IMediator mediator) : IConsumer<Visited>
    {
        private readonly ILogger<CryptoVisitedConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<Visited> context)
        {
            await mediator.Send(new VisitedCommand
            {
                Symbol = context.Message.Symbol, 
                CryptoId = context.Message.CryptoId
            });
        }
    }
}