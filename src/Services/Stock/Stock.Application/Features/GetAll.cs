using MediatR;
using Stock.Application.Interfaces;
using Stock.Core.Queries;

namespace Stock.Application.Features
{
    /// <summary>
    /// Fetch all crypto items with last price tag
    /// </summary>
    public static class GetAll
    {
        public record Query : IRequest<IEnumerable<Response>> { }

        public class Handler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IStockRepository _repo;

            public Handler(IStockRepository repo)
            {
                _repo = repo;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllWithPrice();

                return MapToResponse(items);
            }

            private IEnumerable<Response> MapToResponse(List<StockPriceTagQuery> items)
            {
                return items.Select(i => new Response
                {
                    Id = i.Id,
                    Symbol = i.Symbol,
                    Price = i.Price
                });
            }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Symbol { get; set; }
            public decimal Price { get; set; }
        }
    }
}
