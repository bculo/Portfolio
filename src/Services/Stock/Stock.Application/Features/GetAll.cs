using MediatR;
using Stock.Application.Interfaces;

namespace Stock.Application.Features
{
    public static class GetAll
    {
        public record Query : IRequest<IEnumerable<Response>> { }

        public class Handler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IBaseRepository<Core.Entities.Stock> _repo;

            public Handler(IBaseRepository<Core.Entities.Stock> repo)
            {
                _repo = repo;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAll();
                return MapToResponse(items);
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

        public class Response
        {
            public int Id { get; set; }
            public string Symbol { get; set; }
        }
    }
}
