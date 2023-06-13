using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Stock.Application.Interfaces;
using Stock.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Stock.Application.Features
{
    public static class AddNew
    {
        public record Command : IRequest<long>
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
            private readonly IStockPriceClient _client;
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IBaseRepository<Core.Entities.Stock> _repo;

            public Handler(IStockPriceClient client,
                IPublishEndpoint publishEndpoint,
                IBaseRepository<Core.Entities.Stock> repo)
            {
                _repo = repo;
                _client = client;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingInstance = await _repo.First(i => i.Symbol.ToLower() == request.Symbol.ToLower());
                if (existingInstance is not null)
                {
                    throw new StockCoreException($"Stock with symbol {request.Symbol} already exists in storage");
                }

                var clientResult = await _client.GetPriceForSymbol(request.Symbol);
                if (clientResult is null)
                {
                    throw new StockCoreException($"Stock with symbol {request.Symbol} is not supported");
                }

                var newItem = new Core.Entities.Stock { Symbol = request.Symbol };
                await _repo.Add(newItem);
                await _repo.SaveChanges();

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
