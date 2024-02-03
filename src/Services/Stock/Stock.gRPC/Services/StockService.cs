using Grpc.Core;
using MediatR;
using Stock.Application.Queries.Stock;

namespace Stock.gRPC.Services;

public class StockService : Stock.StockBase
{
    private readonly IMediator _mediator;
    
    public StockService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<StockItemReply> GetById(GetSingleByIdRequest request, ServerCallContext context)
    {
        var instance = await _mediator.Send(new GetStockById(request.Id));

        return new StockItemReply
        {
            Symbol = instance.Symbol,
            Id = instance.Symbol,
            Price = instance.Price
        };
    }

    public override async Task<StockItemReply> GetBySymbol(GetBySymbolRequest request, ServerCallContext context)
    {
        var instance = await _mediator.Send(new GetStockBySymbol(request.Symbol));
        
        return new StockItemReply
        {
            Symbol = instance.Symbol,
            Id = instance.Symbol,
            Price = instance.Price
        };
    }
}