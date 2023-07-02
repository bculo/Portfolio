using Cache.Abstract.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Models;
using Stock.Application.Common.Utilities;
using Stock.Application.Interfaces;
using Stock.Application.Resources.Shared;
using Stock.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Stock.Application.Features
{
    /// <summary>
    /// Fetch single stock item for given symbol with a price tag
    /// </summary>
    public static class GetSingle
    {
        public record Query(string Symbol) : IRequest<Response>;

        public class Validator : AbstractValidator<Query>
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

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ICacheService _cache;
            private readonly ILogger<Handler> _logger;
            private readonly IStringLocalizer<GetSingleLocale> _localizer;

            public Handler(ICacheService cache,
                IStringLocalizer<GetSingleLocale> localizer,
                ILogger<Handler> logger)
            {
                _localizer = localizer;
                _cache = cache;
                _logger = logger;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var cacheItem = await _cache.Get<StockCacheItem>(StringUtilities.AddStockPrefix(request.Symbol));
                if(cacheItem is not null)
                {
                    _logger.LogTrace("Requested symbol {item} fetched from cache", request.Symbol);
                    return ToResponse(cacheItem.Id, cacheItem.Symbol, cacheItem.Price);
                }

                string excMessage = string.Format(_localizer.GetString("Symbol not found"), request.Symbol);
                throw new StockCoreNotFoundException(excMessage);
            }

            private Response ToResponse(long id, string symbol, decimal price)
            {
                return new Response
                {
                    Id = id,
                    Symbol = symbol,
                    Price = price
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

    public class GetSingleLocale { }
}
