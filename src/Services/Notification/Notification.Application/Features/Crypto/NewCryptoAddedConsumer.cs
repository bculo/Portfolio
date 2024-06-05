using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Crypto;

public class NewCryptoAddedConsumer(IMediator mediator) : IConsumer<NewItemAdded>
{
    public async Task Consume(ConsumeContext<NewItemAdded> context)
    {
        var instance = context.Message;
        var command = new SendPriceUpdatedNotification(instance.Symbol, instance.Price);
        await mediator.Send(command);
    }
}