using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Sqids;
using Stock.Application.Common.Constants;
using Stock.Application.Interfaces.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Commands.Cache;

public record RefreshStockItemValue(int Id, string Symbol, decimal Price) : IRequest;


public class RefreshStockItemValueHandler(
    IFusionCache cache,
    SqidsEncoder<int> sqids,
    IOutputCacheStore outputCache,
    IUnitOfWork work)
    : IRequestHandler<RefreshStockItemValue>
{
    public async Task Handle(RefreshStockItemValue request, CancellationToken ct)
    {
        var evictTag = sqids.Encode(request.Id);

        var entity = await work.StockWithPriceTag.First(i => i.StockId == request.Id, 
            orderBy: i => i.OrderByDescending(x => x.CreatedAt),
            ct: ct);
                
        await cache.SetAsync(CacheKeys.StockItemKey(request.Id),
            entity,
            CacheKeys.StockItemKeyOptions(),
            ct);
            
        await outputCache.EvictByTagAsync(evictTag, default);
    }
}