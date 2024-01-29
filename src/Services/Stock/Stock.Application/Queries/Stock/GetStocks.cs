using MediatR;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record GetStocks : IRequest<IEnumerable<GetStocksResponse>> { }

public class GetStocksHandler : IRequestHandler<GetStocks, IEnumerable<GetStocksResponse>>
{
    private readonly IStockRepository _repo;

    public GetStocksHandler(IStockRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<GetStocksResponse>> Handle(GetStocks request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var items = await _repo.GetAllWithPrice();

        //return MapToResponse(items);
    }

    private IEnumerable<GetStocksResponse> MapToResponse(List<StockWithPriceTagReadModel> items)
    {
        return items.Select(i => new GetStocksResponse
        {
            Id = i.StockId,
            Symbol = i.Symbol,
            Price = i.Price
        });
    }
}

public class GetStocksResponse
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}