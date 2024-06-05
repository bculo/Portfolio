using MediatR;
using Sqids;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record GetStocks : IRequest<IEnumerable<GetStocksResponse>>;

public class GetStocksHandler(IUnitOfWork work, SqidsEncoder<int> sqids)
    : IRequestHandler<GetStocks, IEnumerable<GetStocksResponse>>
{
    public async Task<IEnumerable<GetStocksResponse>> Handle(GetStocks request, CancellationToken ct)
    {
        var stocks = await work.StockWithPriceTag.GetAll(ct);
        return MapToResponse(stocks);
    }

    private IEnumerable<GetStocksResponse> MapToResponse(List<StockWithPriceTag> items)
    {
        return items.Select(item => new GetStocksResponse
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price == -1 ? default(double?) : (double)item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = sqids.Encode(item.StockId),
            Created = item.CreatedAt
        });
    }
}

public record GetStocksResponse : StockPriceResultDto;