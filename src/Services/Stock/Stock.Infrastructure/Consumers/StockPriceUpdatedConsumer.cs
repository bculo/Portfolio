using Events.Common.Stock;
using MassTransit;
using Microsoft.AspNetCore.OutputCaching;
using Sqids;
using Stock.Application.Common.Configurations;
using Stock.Application.Interfaces.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Infrastructure.Consumers
{
    public class StockPriceUpdatedConsumer : IConsumer<StockPriceUpdated>
    {
        private readonly IFusionCache _cache;
        private readonly SqidsEncoder<int> _sqids;
        private readonly IOutputCacheStore _outputCache;
        private readonly IUnitOfWork _work;

        public StockPriceUpdatedConsumer(IFusionCache cache, 
            SqidsEncoder<int> sqids, 
            IOutputCacheStore outputCache, 
            IUnitOfWork work)
        {
            _cache = cache;
            _sqids = sqids;
            _outputCache = outputCache;
            _work = work;
        }

        public async Task Consume(ConsumeContext<StockPriceUpdated> context)
        {
            var stockItem = context.Message;
            var evictTag = _sqids.Encode(stockItem.Id);

            var entity = _work.StockRepo.Find(stockItem.Id);
                
            await _cache.SetAsync(CacheKeys.StockItemKey(stockItem.Id),
                entity,
                CacheKeys.StockItemKeyOptions(),
                default);
            
            await _outputCache.EvictByTagAsync(evictTag, default);
        }
    }
}
