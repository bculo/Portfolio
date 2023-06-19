using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces;
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
            private readonly IStockRepository _repo;

            public Handler(IStockRepository repo)
            {
                _repo = repo;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await _repo.GetCurrentPrice(request.Symbol);
                if (item is null)
                {
                    throw new StockCoreNotFoundException($"Given symbol {request.Symbol} not found");
                }

                return new Response
                {
                    Id = item.Id,
                    Symbol = item.Symbol,
                    Price = item.Price
                };
            }
        }

        public class Response
        {
            public long Id { get; set; }
            public string Symbol { get; set; }
            public decimal Price { get; set; }
        }
    }
}
