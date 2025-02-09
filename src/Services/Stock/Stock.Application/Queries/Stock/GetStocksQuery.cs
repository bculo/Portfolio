using MediatR;
using Microsoft.EntityFrameworkCore;
using Sqids;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record GetStocksQuery : IRequest<IEnumerable<GetStocksResponse>>;

public class GetStocksQueryHandler(IDataSourceProvider provider) : IRequestHandler<GetStocksQuery, IEnumerable<GetStocksResponse>>
{
    public async Task<IEnumerable<GetStocksResponse>> Handle(GetStocksQuery request, CancellationToken ct)
    {
        var stocks = await provider.GetReadOnlySourceQuery<StockWithPriceTag>().ToListAsync(ct);
        return Map(stocks);
    }

    private IEnumerable<GetStocksResponse> Map(List<StockWithPriceTag> items)
    {
        return items.Select(item => new GetStocksResponse
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = item.StockId,
            Created = item.CreatedAt
        });
    }
}

public record GetStocksResponse : StockPriceResultDto;