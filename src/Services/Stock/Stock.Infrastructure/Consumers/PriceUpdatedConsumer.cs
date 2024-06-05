using Events.Common.Stock;
using MassTransit;
using MediatR;
using Stock.Application.Commands.Cache;

namespace Stock.Infrastructure.Consumers
{
    public class PriceUpdatedConsumer(IMediator mediator) : IConsumer<StockPriceUpdated>
    {
        public async Task Consume(ConsumeContext<StockPriceUpdated> context)
        {
            var body = context.Message;
            await mediator.Send(new RefreshStockItemValue(body.Id, body.Symbol, body.Price));
        }
    }
}
