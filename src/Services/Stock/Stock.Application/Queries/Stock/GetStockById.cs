using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sqids;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Queries.Stock;


public record GetStockById(string Id) : IRequest<GetStockByIdResponse>;

public class GetStockByIdValidator : AbstractValidator<GetStockById>
{
    public GetStockByIdValidator(ILocale locale)
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}

public class GetStockByIdHandler(
    ILogger<GetStockByIdHandler> logger,
    IUnitOfWork work,
    SqidsEncoder<int> sqids,
    IFusionCache cache)
    : IRequestHandler<GetStockById, GetStockByIdResponse>
{
    private readonly ILogger<GetStockByIdHandler> _logger = logger;

    public async Task<GetStockByIdResponse> Handle(GetStockById request, CancellationToken ct)
    {
        var entityId = sqids.DecodeSingle(request.Id);
        if (entityId == default(long))
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        }

        var entity = await cache.GetOrSetAsync(
            CacheKeys.StockItemKey(entityId),
            token => work.StockWithPriceTag.First(i => i.StockId == entityId, 
                orderBy: i => i.OrderByDescending(x => x.CreatedAt),
                ct: token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        if(entity is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        }
        
        return Map(entity);
    }

    private GetStockByIdResponse Map(StockWithPriceTag item)
    {
        return new GetStockByIdResponse
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

public record GetStockByIdResponse : StockPriceResultDto;