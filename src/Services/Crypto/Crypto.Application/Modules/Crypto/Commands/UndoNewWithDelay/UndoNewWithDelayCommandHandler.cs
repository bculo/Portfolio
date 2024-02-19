using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay
{
    public class UndoNewWithDelayCommandHandler : IRequestHandler<UndoNewWithDelayCommand>
    {
        private readonly IPublishEndpoint _publish;

        public UndoNewWithDelayCommandHandler(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        public async Task Handle(UndoNewWithDelayCommand request, CancellationToken cancellationToken)
        {
            await _publish.Publish(new UndoAddCryptoItemWithDelay
            {
                TemporaryId = request.TemporaryId,
            }, cancellationToken);
        }
    }
}
