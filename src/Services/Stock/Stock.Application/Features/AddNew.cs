using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Application.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace Stock.Application.Features
{
    public static class AddNew
    {
        public class Command : IRequest<long>
        {
            public string Symbol { get; set; }
        }

        public class Validator : AbstractValidator<Command>
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

        public class Handler : IRequestHandler<Command, long>
        {
            private readonly ILogger<Handler> _logger;
            private readonly StockDbContext _db;

            public Handler(ILogger<Handler> logger, StockDbContext db)
            {
                _logger = logger;
                _db = db;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingInstance = await _db.Stocks.FirstOrDefaultAsync(i => i.Symbol.ToLower() == request.Symbol.ToLower());
                if(existingInstance is not null)
                {
                    _logger.LogWarning("Stock with symbol {symbol} already exists in storage", request.Symbol);
                    throw new Exception($"Stock with symbol {request.Symbol} already exists in storage");
                }

                var newItem = new Core.Entities.Stock { Symbol = request.Symbol };
                _db.Stocks.Add(newItem);
                await _db.SaveChangesAsync();

                return newItem.Id;
            }
        }
    }
}
