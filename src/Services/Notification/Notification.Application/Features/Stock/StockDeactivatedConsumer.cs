using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class StockDeactivatedConsumer : IConsumer<StockDeactivated>
{
    private readonly IMediator _mediator;

    public StockDeactivatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<StockDeactivated> context)
    {
        var instance = context.Message;
        var command = new StockStatusChangedNotification(instance.Symbol, instance.Time, false);
        await _mediator.Send(command);
    }
}