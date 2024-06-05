using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class NewStockAddedConsumer(IMediator mediator) : PriceUpdatedBaseConsumer, IConsumer<NewStockItemAdded>
{
    public async Task Consume(ConsumeContext<NewStockItemAdded> context)
    {
        var instance = context.Message;
        var command = CreateNotification(instance.Symbol, instance.Price);
        await mediator.Send(command);
    }
}