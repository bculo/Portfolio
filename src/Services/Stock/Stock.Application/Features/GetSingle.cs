using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Application.Infrastructure.Persistence;
using Stock.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Stock.Application.Features
{
    public static class GetSingle
    {
        public record Query : IRequest<Response>
        {
            public string Symbol { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(i => i.Symbol)
                    .Matches(new Regex("^[a-zA-Z]+$",
                        RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1)))
                    .MaximumLength(10)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly StockDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger, StockDbContext context)
            {
                _logger = logger;
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await _context.Stocks.FirstOrDefaultAsync(i => i.Symbol.ToLower() ==  request.Symbol.ToLower(), cancellationToken);
                if(item is null)
                {
                    throw new StockCoreNotFoundException($"Given symbol {request.Symbol} not found");
                }

                return new Response
                {
                    Id = item.Id,
                    Symbol = item.Symbol,
                };
            }
        }

        public class Response
        {
            public long Id { get; set; }
            public string Symbol { get; set; }
        }
    }
}
