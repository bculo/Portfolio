using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

public record GetStockBySymbolQuery(string Symbol) : IRequest<GetStockBySymbolResponse>;


public class GetStockBySymbolQueryValidator : AbstractValidator<GetStockBySymbolQuery>
{
    public GetStockBySymbolQueryValidator(ILocale locale)
    {
        RuleFor(i => i.Symbol)
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.StockSymbolPattern))
            .NotEmpty();
    }
}

public class GetStockBySymbolQueryHandler(
    IDataSourceProvider queryProvider,
    IFusionCache fusionCache)
    : IRequestHandler<GetStockBySymbolQuery, GetStockBySymbolResponse>
{
    public async Task<GetStockBySymbolResponse> Handle(GetStockBySymbolQuery request, CancellationToken ct)
    {
        var instance = await fusionCache.GetOrSetAsync(
            CacheKeys.StockItemKey(request.Symbol),
            token => queryProvider.GetReadOnlySourceQuery<StockWithPriceTag>()
                .FirstOrDefaultAsync(x => x.Symbol == request.Symbol, token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        
        if(instance is null)
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundBySymbol(request.Symbol));
        
        return Map(instance);
    }
    
    private GetStockBySymbolResponse Map(StockWithPriceTag item)
    {
        return new GetStockBySymbolResponse
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = item.StockId,
            Created = item.CreatedAt
        };
    }
}

public record GetStockBySymbolResponse : StockPriceResultDto;