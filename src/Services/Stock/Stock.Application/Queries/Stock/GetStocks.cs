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
        throw new NotImplementedException();
        //var items = await _repo.GetAllWithPrice();

        //return MapToResponse(items);
    }

    private IEnumerable<GetStocksResponse> MapToResponse(List<StockWithPriceTagReadModel> items)
    {
        return items.Select(i => new GetStocksResponse(Id: i.StockId, Symbol: i.Symbol, Price: i.Price));
    }
}

public record GetStocksResponse(long Id, string Symbol, decimal Price);