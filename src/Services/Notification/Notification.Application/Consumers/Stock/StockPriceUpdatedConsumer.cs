using Events.Common.Crypto;
using Events.Common.Stock;
using MassTransit;
using MassTransit.Mediator;

namespace Notification.Application.Consumers.Stock;

public class StockPriceUpdatedConsumer : IConsumer<StockPriceUpdated>
{
    private readonly IMediator _mediator;

    public StockPriceUpdatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<StockPriceUpdated> context)
    {
        var instance = context.Message;
        var command = new Commands.Stock.SendPriceUpdatedNotification(instance.Symbol, instance.Price);
        await _mediator.Send(command);
    }
}