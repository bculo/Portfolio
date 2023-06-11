using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Infrastructure.Persistence;

namespace Stock.Application.Features
{
    public static class GetAll
    {
        public record Query : IRequest<IEnumerable<Response>> { }

        public class Handler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly StockDbContext _context;

            public Handler(StockDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Stocks.ToListAsync(cancellationToken);
                if(!result.Any())
                {
                    return Enumerable.Empty<Response>();
                }

                return result.Select(i => new Response
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
