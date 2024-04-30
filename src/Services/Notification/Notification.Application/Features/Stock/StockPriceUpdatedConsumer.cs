using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class StockPriceUpdatedConsumer : PriceUpdatedBaseConsumer, IConsumer<StockPriceUpdated>
{
    private readonly IMediator _mediator;

    public StockPriceUpdatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<StockPriceUpdated> context)
    {
        var instance = context.Message;
        var command = CreateNotification(instance.Symbol, instance.Price);
        await _mediator.Send(command);
    }
}