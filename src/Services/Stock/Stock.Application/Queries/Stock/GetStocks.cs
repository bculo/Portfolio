using MediatR;
using Sqids;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record GetStocks : IRequest<IEnumerable<GetStocksResponse>>;

public class GetStocksHandler : IRequestHandler<GetStocks, IEnumerable<GetStocksResponse>>
{
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;

    public GetStocksHandler(IUnitOfWork work, SqidsEncoder<int> sqids)
    {
        _work = work;
        _sqids = sqids;
    }

    public async Task<IEnumerable<GetStocksResponse>> Handle(
        GetStocks request, 
        CancellationToken ct)
    {
        var stocks = await _work.StockWithPriceTag.GetAll(ct);
        return MapToResponse(stocks);
    }

    private IEnumerable<GetStocksResponse> MapToResponse(List<StockWithPriceTag> items)
    {
        return items.Select(i => new GetStocksResponse(_sqids.Encode(i.StockId), i.Symbol, i.Price));
    }
}

public record GetStocksResponse(string Id, string Symbol, decimal Price);