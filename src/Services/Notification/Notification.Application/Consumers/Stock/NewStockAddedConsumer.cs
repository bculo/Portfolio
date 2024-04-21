using Events.Common.Stock;
using MassTransit;
using MassTransit.Mediator;
using Notification.Application.Commands.Stock;

namespace Notification.Application.Consumers.Stock;

public class NewStockAddedConsumer : IConsumer<NewStockItemAdded>
{
    private readonly IMediator _mediator;

    public NewStockAddedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<NewStockItemAdded> context)
    {
        var instance = context.Message;
        var command = new SendPriceUpdatedNotification(instance.Symbol, instance.Price);
        await _mediator.Send(command);
    }
}