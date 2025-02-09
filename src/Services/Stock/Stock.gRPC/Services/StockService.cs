using Grpc.Core;
using MediatR;
using Stock.Application.Queries.Stock;

namespace Stock.gRPC.Services;

public class StockService(IMediator mediator) : Stock.StockBase
{
    public override async Task<StockItemReply> GetById(GetSingleByIdRequest request, ServerCallContext context)
    {
        var instance = await mediator.Send(new GetStockByIdQuery(request.Id));

        return new StockItemReply
        {
            Symbol = instance.Symbol,
            Id = instance.Symbol,
            Price = instance.Price
        };
    }

    public override async Task<StockItemReply> GetBySymbol(GetBySymbolRequest request, ServerCallContext context)
    {
        var instance = await mediator.Send(new GetStockBySymbol(request.Symbol));
        
        return new StockItemReply
        {
            Symbol = instance.Symbol,
            Id = instance.Symbol,
            Price = instance.Price
        };
    }
}