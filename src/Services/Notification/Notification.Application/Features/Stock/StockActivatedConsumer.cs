using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class StockActivatedConsumer : IConsumer<StockActivated>
{
    private readonly IMediator _mediator;

    public StockActivatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<StockActivated> context)
    {
        var instance = context.Message;
        var command = new StockStatusChangedNotification(instance.Symbol, instance.Time, true);
        await _mediator.Send(command);
    }
}