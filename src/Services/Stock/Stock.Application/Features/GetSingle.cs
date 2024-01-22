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

namespace Stock.Application.Features;

public record GetSingleQuery(string Symbol) : IRequest<GetSingleResponse>;

public class GetSingleValidator : AbstractValidator<GetSingleQuery>
{
    public GetSingleValidator(IStringLocalizer<ValidationShared> localizer)
    {
        RuleFor(i => i.Symbol)
            .Matches(new Regex("^[a-zA-Z]{1,10}$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                TimeSpan.FromSeconds(1)))
            .WithMessage(localizer.GetString("Symbol pattern not valid"))
            .NotEmpty();
    }
}

public class GetSingleHandler : IRequestHandler<GetSingleQuery, GetSingleResponse>
{
    private readonly ICacheService _cache;
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<GetSingleHandler> _logger;
    private readonly IStringLocalizer<GetSingleLocale> _localizer;

    public GetSingleHandler(ICacheService cache,
        IStringLocalizer<GetSingleLocale> localizer,
        ILogger<GetSingleHandler> logger, 
        IStockRepository stockRepository)
    {
        _localizer = localizer;
        _cache = cache;
        _logger = logger;
        _stockRepository = stockRepository;
    }

    public async Task<GetSingleResponse> Handle(GetSingleQuery request, CancellationToken cancellationToken)
    {
        var item = await _stockRepository.First(x => x.Symbol == request.Symbol);
        if(item is not null)
        {
            _logger.LogTrace("Requested symbol {item} fetched from cache", request.Symbol);
            return ToResponse(item.Id, item.Symbol, 0);
        }

        string excMessage = string.Format(_localizer.GetString("Symbol not found"), request.Symbol);
        throw new StockCoreNotFoundException(excMessage);
    }

    private GetSingleResponse ToResponse(long id, string symbol, decimal price)
    {
        return new GetSingleResponse
        {
            Id = id,
            Symbol = symbol,
            Price = price
        };
    }
}

public class GetSingleResponse
{
    public long Id { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}


public class GetSingleLocale { }

