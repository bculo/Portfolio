using Cache.Abstract.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;


public record GetStock(string Symbol) : IRequest<GetStockResponse>;

public class GetStockValidator : AbstractValidator<GetStock>
{
    public GetStockValidator(ILocale locale)
    {
        RuleFor(i => i.Symbol)
            .NotEmpty()
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.STOCK_SYMBOL_PATTERN));
    }
}

public class GetStockHandler : IRequestHandler<GetStock, GetStockResponse>
{
    private readonly ICacheService _cache;
    private readonly IUnitOfWork _work;
    private readonly ILogger<GetStockHandler> _logger;

    public GetStockHandler(ICacheService cache,
        ILogger<GetStockHandler> logger, 
        IUnitOfWork work)
    {
        _cache = cache;
        _logger = logger;
        _work = work;
    }
    
    public async Task<GetStockResponse> Handle(GetStock request, CancellationToken ct)
    {
        var entity = await _work.StockRepo.First(
            x => x.Symbol.ToLower() == request.Symbol.ToLower(),
            ct: ct);
        
        if(entity is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundBySymbol(request.Symbol));
        }
        
        return Map(entity);
    }

    private GetStockResponse Map(StockEntity entity)
    {
        return new GetStockResponse(Id: entity.Id, Symbol: entity.Symbol);
    }
}

public record GetStockResponse(long Id, string Symbol);