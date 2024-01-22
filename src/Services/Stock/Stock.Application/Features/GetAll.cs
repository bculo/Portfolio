using MediatR;
using Stock.Application.Interfaces;
using Stock.Core.Queries;

namespace Stock.Application.Features;

public record GetAllQuery : IRequest<IEnumerable<GetAllResponse>> { }

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<GetAllResponse>>
{
    private readonly IStockRepository _repo;

    public GetAllHandler(IStockRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<GetAllResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllWithPrice();

        return MapToResponse(items);
    }

    private IEnumerable<GetAllResponse> MapToResponse(List<StockPriceTagQuery> items)
    {
        return items.Select(i => new GetAllResponse
        {
            Id = i.Id,
            Symbol = i.Symbol,
            Price = i.Price
        });
    }
}

public class GetAllResponse
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}
