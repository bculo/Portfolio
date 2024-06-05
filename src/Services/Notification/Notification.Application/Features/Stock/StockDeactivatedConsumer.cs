using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class StockDeactivatedConsumer(IMediator mediator) : IConsumer<StockDeactivated>
{
    public async Task Consume(ConsumeContext<StockDeactivated> context)
    {
        var instance = context.Message;
        var command = new StockStatusChangedNotification(instance.Symbol, instance.Time, false);
        await mediator.Send(command);
    }
}