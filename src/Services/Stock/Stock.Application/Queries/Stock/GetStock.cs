using System.Text.RegularExpressions;
using Cache.Abstract.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources.Shared;
using Stock.Core.Exceptions;

namespace Stock.Application.Queries.Stock;


public record GetStock(string Symbol) : IRequest<GetStockResponse>;

public class GetStockValidator : AbstractValidator<GetStock>
{
    public GetStockValidator(IStringLocalizer<ValidationShared> localizer)
    {
        RuleFor(i => i.Symbol)
            .Matches(new Regex("^[a-zA-Z]{1,10}$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                TimeSpan.FromSeconds(1)))
            .WithMessage(localizer.GetString("Symbol pattern not valid"))
            .NotEmpty();
    }
}

public class GetStockHandler : IRequestHandler<GetStock, GetStockResponse>
{
    private readonly ICacheService _cache;
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<GetStockHandler> _logger;
    private readonly IStringLocalizer<GetStockLocale> _localizer;

    public GetStockHandler(ICacheService cache,
        IStringLocalizer<GetStockLocale> localizer,
        ILogger<GetStockHandler> logger, 
        IStockRepository stockRepository)
    {
        _localizer = localizer;
        _cache = cache;
        _logger = logger;
        _stockRepository = stockRepository;
    }

    public async Task<GetStockResponse> Handle(GetStock request, CancellationToken cancellationToken)
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

    private GetStockResponse ToResponse(long id, string symbol, decimal price)
    {
        return new GetStockResponse
        {
            Id = id,
            Symbol = symbol,
            Price = price
        };
    }
}

public class GetStockResponse
{
    public long Id { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}


public class GetStockLocale { }