using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class NewStockAddedConsumer : PriceUpdatedBaseConsumer, IConsumer<NewStockItemAdded>
{
    private readonly IMediator _mediator;

    public NewStockAddedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<NewStockItemAdded> context)
    {
        var instance = context.Message;
        var command = CreateNotification(instance.Symbol, instance.Price);
        await _mediator.Send(command);
    }
}