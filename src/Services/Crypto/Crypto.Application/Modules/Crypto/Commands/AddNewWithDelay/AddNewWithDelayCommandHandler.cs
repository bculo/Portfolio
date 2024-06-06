using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayCommandHandler(
        ILogger<AddNewWithDelayCommandHandler> logger,
        IPublishEndpoint publish)
        : IRequestHandler<AddNewWithDelayCommand, Guid>
    {
        private readonly ILogger<AddNewWithDelayCommandHandler> _logger = logger;

        public async Task<Guid> Handle(AddNewWithDelayCommand request, CancellationToken cancellationToken)
        {
            var temporaryId = Guid.NewGuid();

            await publish.Publish(new AddItemWithDelay
            {
                Symbol = request.Symbol,
                CorrelationId = temporaryId
            }, cancellationToken);

            return temporaryId;
        }
    }
}
