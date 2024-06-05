using FluentValidation;
using MediatR;
using Sqids;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
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

public class GetStockBySymbolHandler(
    IUnitOfWork work,
    SqidsEncoder<int> sqids,
    IFusionCache fusionCache)
    : IRequestHandler<GetStockBySymbol, GetStockBySymbolResponse>
{
    public async Task<GetStockBySymbolResponse> Handle(GetStockBySymbol request, CancellationToken ct)
    {
        var instance = await fusionCache.GetOrSetAsync(
            CacheKeys.StockItemKey(request.Symbol),
            token => work.StockWithPriceTag.First(i => i.Symbol.ToLower() == request.Symbol.ToLower(), ct: token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        
        if(instance is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundBySymbol(request.Symbol));
        }
        
        return Map(instance);
    }
    
    private GetStockBySymbolResponse Map(StockWithPriceTag item)
    {
        return new GetStockBySymbolResponse
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price == -1 ? default(double?) : (double)item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = sqids.Encode(item.StockId),
            Created = item.CreatedAt
        };
    }
}

public record GetStockBySymbolResponse : StockPriceResultDto;