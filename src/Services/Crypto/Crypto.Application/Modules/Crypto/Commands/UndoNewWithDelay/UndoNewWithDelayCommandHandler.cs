using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay
{
    public class UndoNewWithDelayCommandHandler : IRequestHandler<UndoNewWithDelayCommand>
    {
        private readonly IPublishEndpoint _publish;

        public UndoNewWithDelayCommandHandler(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        public async Task<Unit> Handle(UndoNewWithDelayCommand request, CancellationToken cancellationToken)
        {
            await _publish.Publish(new UndoAddCryptoItemWithDelay
            {
                TemporaryId = request.TemporaryId,
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
