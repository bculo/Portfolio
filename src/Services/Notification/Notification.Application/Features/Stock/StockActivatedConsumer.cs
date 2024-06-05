using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class StockActivatedConsumer(IMediator mediator) : IConsumer<StockActivated>
{
    public async Task Consume(ConsumeContext<StockActivated> context)
    {
        var instance = context.Message;
        var command = new StockStatusChangedNotification(instance.Symbol, instance.Time, true);
        await mediator.Send(command);
    }
}