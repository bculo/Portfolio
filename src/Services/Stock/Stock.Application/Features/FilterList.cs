using MediatR;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces;

namespace Stock.Application.Features
{
    public static class FilterList
    {
        public record Query : PageRequest, IRequest<IEnumerable<Response>>
        {
            public string Symbol { get; set; }
        }

        public class Validator : PageBaseValidator<Query> { }

        public class Handler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IBaseRepository<Core.Entities.Stock> _repo;

            public Handler(IBaseRepository<Core.Entities.Stock> repo)
            {
                _repo = repo;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await FilterItems(request);
                return MapToResponse(items);
            }

            private async Task<List<Core.Entities.Stock>> FilterItems(Query request)
            {
                var (count, page) = await _repo.Page(
                    i => string.IsNullOrWhiteSpace(request.Symbol) || i.Symbol.Contains(request.Symbol),
                    request.Page,
                    request.Take);

                return page;
            }

            private IEnumerable<Response> MapToResponse(List<Core.Entities.Stock> items)
            {
                return items.Select(i => new Response
                {
                    Id = i.Id,
                    Symbol = i.Symbol,
                });
            }
        }

        public record Response
        {
            public int Id { get; set; }
            public string Symbol { get; set; }
        }
    }
}
