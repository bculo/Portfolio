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
        var instance = await _mediator.Send(new GetStock(request.Id));

        return new StockItemReply
        {
            Symbol = instance.Symbol
        };
    }

    public override Task<StockItemReply> GetBySymbol(GetBySymbolRequest request, ServerCallContext context)
    {
        return base.GetBySymbol(request, context);
    }
}