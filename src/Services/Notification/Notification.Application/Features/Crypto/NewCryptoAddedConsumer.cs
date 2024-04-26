using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Crypto;

public class NewCryptoAddedConsumer : IConsumer<NewItemAdded>
{
    private readonly IMediator _mediator;

    public NewCryptoAddedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<NewItemAdded> context)
    {
        var instance = context.Message;
        var command = new SendPriceUpdatedNotification(instance.Symbol, instance.Price);
        await _mediator.Send(command);
    }
}