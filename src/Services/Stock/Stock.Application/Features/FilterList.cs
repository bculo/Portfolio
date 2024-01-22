using MediatR;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces;

namespace Stock.Application.Features;

public record FilterListQuery(string Symbol) : PageRequest, IRequest<IEnumerable<FilterListResponse>>;

public class FilterListValidator : PageBaseValidator<FilterListQuery> { }

public class FilterListHandler : IRequestHandler<FilterListQuery, IEnumerable<FilterListResponse>>
{
    private readonly IBaseRepository<Core.Entities.Stock> _repo;

    public FilterListHandler(IBaseRepository<Core.Entities.Stock> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FilterListResponse>> Handle(FilterListQuery request, CancellationToken cancellationToken)
    {
        var items = await FilterItems(request);
        return MapToResponse(items);
    }

    private async Task<List<Core.Entities.Stock>> FilterItems(FilterListQuery request)
    {
        var (count, page) = await _repo.Page(
            i => string.IsNullOrWhiteSpace(request.Symbol) || i.Symbol.Contains(request.Symbol),
            request.Page,
            request.Take);

        return page;
    }

    private IEnumerable<FilterListResponse> MapToResponse(List<Core.Entities.Stock> items)
    {
        return items.Select(i => new FilterListResponse
        {
            Id = i.Id,
            Symbol = i.Symbol,
        });
    }
}

public record FilterListResponse
{
    public int Id { get; set; }
    public string Symbol { get; set; }
}
    

