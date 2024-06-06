using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay
{
    public class UndoNewWithDelayCommandHandler(IPublishEndpoint publish) : IRequestHandler<UndoNewWithDelayCommand>
    {
        public async Task Handle(UndoNewWithDelayCommand request, CancellationToken cancellationToken)
        {
            await publish.Publish(new UndoAddItemWithDelay
            {
                CorrelationId = request.TemporaryId,
            }, cancellationToken);
        }
    }
}
