using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Application.Infrastructure.Persistence;
using Stock.Application.Interfaces;
using Stock.Core.Exceptions;
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
            private readonly StockDbContext _db;
            private readonly ILogger<Handler> _logger;
            private readonly IStockPriceClient _client;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(ILogger<Handler> logger,
                StockDbContext db,
                IStockPriceClient client,
                IPublishEndpoint publishEndpoint)
            {
                _db = db;
                _logger = logger;
                _client = client;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingInstance = await _db.Stocks.FirstOrDefaultAsync(i => i.Symbol.ToLower() == request.Symbol.ToLower());
                if(existingInstance is not null)
                {
                    throw new StockCoreException($"Stock with symbol {request.Symbol} already exists in storage");
                }

                var clientResult = await _client.GetPriceForSymbol(request.Symbol);
                if(clientResult is null)
                {
                    throw new StockCoreException($"Stock with symbol {request.Symbol} is not supported");
                }

                var newItem = new Core.Entities.Stock { Symbol = request.Symbol };
                _db.Stocks.Add(newItem);
                await _db.SaveChangesAsync();

                await _publishEndpoint.Publish(new NewStockItemAdded
                {
                    Created = newItem.CreatedAt,
                    Symbol = newItem.Symbol
                });

                return newItem.Id;
            }
        }
    }
}
