using FluentValidation;
using MediatR;
using Sqids;
using Stock.Application.Common.Configurations;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Queries.Stock;

public record GetStockBySymbol(string Symbol) : IRequest<GetStockBySymbolResponse>;


public class GetStockBySymbolValidator : AbstractValidator<GetStockBySymbol>
{
    public GetStockBySymbolValidator(ILocale locale)
    {
        RuleFor(i => i.Symbol)
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.STOCK_SYMBOL_PATTERN))
            .NotEmpty();
    }
}

public class GetStockBySymbolHandler : IRequestHandler<GetStockBySymbol, GetStockBySymbolResponse>
{
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;
    private readonly IFusionCache _fusionCache;

    public GetStockBySymbolHandler(IUnitOfWork work, 
        SqidsEncoder<int> sqids, 
        IFusionCache fusionCache)
    {
        _work = work;
        _sqids = sqids;
        _fusionCache = fusionCache;
    }

    public async Task<GetStockBySymbolResponse> Handle(GetStockBySymbol request, CancellationToken ct)
    {
        var instance = await _fusionCache.GetOrSetAsync(
            CacheKeys.StockItemKey(request.Symbol),
            token => _work.StockWithPriceTag.First(i => i.Symbol.ToLower() == request.Symbol.ToLower(), ct: token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        
        if(instance is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundBySymbol(request.Symbol));
        }
        
        return Map(instance);
    }
    
    private GetStockBySymbolResponse Map(StockWithPriceTag instance)
    {
        return new GetStockBySymbolResponse(_sqids.Encode(instance.StockId), instance.Symbol, instance.Price);
    }
}

public record GetStockBySymbolResponse(string Id, string Symbol, decimal Price);