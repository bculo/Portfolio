using MediatR;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Persistence;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(string Symbol) : PageRequest, IRequest<IEnumerable<FilterStocksResponse>>;

public class FilterStocksValidator : PageBaseValidator<FilterStocks> { }

public class FilterStocksHandler : IRequestHandler<FilterStocks, IEnumerable<FilterStocksResponse>>
{
    private readonly IBaseRepository<Core.Entities.Stock> _repo;

    public FilterStocksHandler(IBaseRepository<Core.Entities.Stock> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FilterStocksResponse>> Handle(FilterStocks request, CancellationToken cancellationToken)
    {
        var items = await FilterItems(request);
        return MapToResponse(items);
    }

    private async Task<List<Core.Entities.Stock>> FilterItems(FilterStocks request)
    {
        var (count, page) = await _repo.Page(
            i => string.IsNullOrWhiteSpace(request.Symbol) || i.Symbol.Contains(request.Symbol),
            request.Page,
            request.Take);

        return page;
    }

    private IEnumerable<FilterStocksResponse> MapToResponse(List<Core.Entities.Stock> items)
    {
        return items.Select(i => new FilterStocksResponse
        {
            Id = i.Id,
            Symbol = i.Symbol,
        });
    }
}

public record FilterStocksResponse
{
    public int Id { get; set; }
    public string Symbol { get; set; }
}
