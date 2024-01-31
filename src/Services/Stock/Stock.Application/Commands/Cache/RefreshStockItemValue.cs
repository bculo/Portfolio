using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Sqids;
using Stock.Application.Common.Configurations;
using Stock.Application.Interfaces.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Commands.Cache;

public record RefreshStockItemValue(int Id, string Symbol, decimal Price) : IRequest;


public class RefreshStockItemValueHandler : IRequestHandler<RefreshStockItemValue>
{
    private readonly IFusionCache _cache;
    private readonly SqidsEncoder<int> _sqids;
    private readonly IOutputCacheStore _outputCache;
    private readonly IUnitOfWork _work;

    public RefreshStockItemValueHandler(IFusionCache cache, 
        SqidsEncoder<int> sqids, 
        IOutputCacheStore outputCache, 
        IUnitOfWork work)
    {
        _cache = cache;
        _sqids = sqids;
        _outputCache = outputCache;
        _work = work;
    }
    
    public async Task Handle(RefreshStockItemValue request, CancellationToken cancellationToken)
    {
        var evictTag = _sqids.Encode(request.Id);

        var entity = await _work.StockRepo.Find(request.Id, cancellationToken);
                
        await _cache.SetAsync(CacheKeys.StockItemKey(request.Id),
            entity,
            CacheKeys.StockItemKeyOptions(),
            default);
            
        await _outputCache.EvictByTagAsync(evictTag, default);
    }
}