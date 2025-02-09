using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Queries.Stock;


public record GetStockByIdQuery(Guid Id) : IRequest<GetStockByIdResponse>;

public class GetStockByIdQueryValidator : AbstractValidator<GetStockByIdQuery>
{
    public GetStockByIdQueryValidator(ILocale locale)
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}

public class GetStockByIdHandler(IDataSourceProvider provider, IFusionCache cache)
    : IRequestHandler<GetStockByIdQuery, GetStockByIdResponse>
{
    public async Task<GetStockByIdResponse> Handle(GetStockByIdQuery request, CancellationToken ct)
    {
        var entity = await cache.GetOrSetAsync(
            CacheKeys.StockItemKey(request.Id),
            token => provider.GetReadOnlySourceQuery<StockWithPriceTag>()
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(x => x.StockId == request.Id, token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        
        if(entity is null)
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        
        return Map(entity);
    }

    private GetStockByIdResponse Map(StockWithPriceTag item)
    {
        return new GetStockByIdResponse
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

public record GetStockByIdResponse : StockPriceResultDto;