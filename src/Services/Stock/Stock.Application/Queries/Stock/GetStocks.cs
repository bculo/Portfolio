using MediatR;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record GetStocks : IRequest<IEnumerable<GetStocksResponse>>;

public class GetStocksHandler : IRequestHandler<GetStocks, IEnumerable<GetStocksResponse>>
{
    private readonly IUnitOfWork _work;

    public GetStocksHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task<IEnumerable<GetStocksResponse>> Handle(
        GetStocks request, 
        CancellationToken ct)
    {
        var stocks = await _work.StockWithPriceTag.GetAll(ct);
        return MapToResponse(stocks);
    }

    private IEnumerable<GetStocksResponse> MapToResponse(List<StockWithPriceTagReadModel> items)
    {
        return items.Select(i => new GetStocksResponse(Id: i.StockId, Symbol: i.Symbol, Price: i.Price));
    }
}

public record GetStocksResponse(long Id, string Symbol, decimal Price);