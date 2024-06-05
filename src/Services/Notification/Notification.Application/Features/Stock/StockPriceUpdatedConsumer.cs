using Events.Common.Stock;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Stock;

public class StockPriceUpdatedConsumer(IMediator mediator) : PriceUpdatedBaseConsumer, IConsumer<StockPriceUpdated>
{
    public async Task Consume(ConsumeContext<StockPriceUpdated> context)
    {
        var instance = context.Message;
        var command = CreateNotification(instance.Symbol, instance.Price);
        await mediator.Send(command);
    }
}