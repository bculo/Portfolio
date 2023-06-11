using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Models;
using Stock.Application.Infrastructure.Persistence;

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
            private readonly StockDbContext _dbContext;

            public Handler(StockDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = BuildQuery(request);
                
                var items = await query.AsNoTracking().ToListAsync(cancellationToken);

                return MapToResponse(items);
            }

            private IQueryable<Core.Entities.Stock> BuildQuery(Query request)
            {
                var query = _dbContext.Stocks.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Symbol))
                {
                    query = query.Where(i => i.Symbol.Contains(request.Symbol));
                }

                query = query.OrderBy(i => i.Symbol)
                    .Skip((request.Page - 1) * request.Take)
                    .Take(request.Take);

                return query;
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
