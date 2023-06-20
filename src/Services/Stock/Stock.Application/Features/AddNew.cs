using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Localization;
using Stock.Application.Interfaces;
using Stock.Application.Resources.Shared;
using Stock.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Stock.Application.Features
{
    /// <summary>
    /// All nedded logic for adding new stock item to storage
    /// </summary>
    public static class AddNew
    {
        public record Command : IRequest<long>
        {
            public string Symbol { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IStringLocalizer<ValidationShared> localizer)
            {
                RuleFor(i => i.Symbol)
                    .Matches(new Regex("^[a-zA-Z]{1,10}$",
                        RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1)))
                    .WithMessage(localizer.GetString("Symbol pattern not valid"))
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, long>
        {
            private readonly IStockPriceClient _client;
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IStringLocalizer<AddNewLocale> _localizer;
            private readonly IBaseRepository<Core.Entities.Stock> _repo;

            public Handler(IStockPriceClient client,
                IPublishEndpoint publishEndpoint,
                IBaseRepository<Core.Entities.Stock> repo,
                IStringLocalizer<AddNewLocale> localizer)
            {
                _repo = repo;
                _client = client;
                _publishEndpoint = publishEndpoint;
                _localizer = localizer;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingInstance = await _repo.First(i => i.Symbol.ToLower() == request.Symbol.ToLower());
                if (existingInstance is not null)
                {
                    string exceptionMessage = string.Format(_localizer.GetString("Symbol already exists"), request.Symbol);
                    throw new StockCoreException(exceptionMessage);
                }

                var clientResult = await _client.GetPriceForSymbol(request.Symbol);
                if (clientResult is null)
                {
                    string exceptionMessage = string.Format(_localizer.GetString("Symbol not supported"), request.Symbol);
                    throw new StockCoreException(exceptionMessage);
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

    public class AddNewLocale { }
}
