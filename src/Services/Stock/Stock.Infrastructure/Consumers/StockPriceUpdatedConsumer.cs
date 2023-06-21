using Cache.Abstract.Contracts;
using Events.Common.Stock;
using MassTransit;
using Stock.Application.Common.Models;
using Stock.Application.Common.Utilities;

namespace Stock.Infrastructure.Consumers
{
    public class StockPriceUpdatedConsumer : IConsumer<StockPriceUpdated>
    {
        private readonly ICacheService _cache;

        public StockPriceUpdatedConsumer(ICacheService cache)
        {
            _cache = cache;
        }

        public Task Consume(ConsumeContext<StockPriceUpdated> context)
        {
            var cacheItem = ToCacheItem(context.Message);
            _cache.Add(StringUtilities.AddStockPrefix(cacheItem.Symbol), cacheItem);
            return Task.CompletedTask;
        }

        public StockCacheItem ToCacheItem(StockPriceUpdated item)
        {
            return new StockCacheItem
            {
                Symbol = item.Symbol,
                Id = item.Id,
                Price = item.Price,
            };
        }
    }
}
