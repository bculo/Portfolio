using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayCommandHandler : IRequestHandler<AddNewWithDelayCommand, Guid>
    {
        private readonly ILogger<AddNewWithDelayCommandHandler> _logger;
        private readonly IPublishEndpoint _publish;

        public AddNewWithDelayCommandHandler(ILogger<AddNewWithDelayCommandHandler> logger,
            IPublishEndpoint publish)
        {
            _logger = logger;
            _publish = publish;
        }

        public async Task<Guid> Handle(AddNewWithDelayCommand request, CancellationToken cancellationToken)
        {
            var temporaryID = Guid.NewGuid();

            await _publish.Publish(new AddCryptoItemWithDelay
            {
                Symbol = request.Symbol,
                TemporaryId = temporaryID
            }, cancellationToken);

            return temporaryID;
        }
    }
}
