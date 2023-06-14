using Events.Common.Stock;
using MassTransit;
using Tracker.Application.Common.Models;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.Infrastructure.Consumers
{
    public class StockPriceUpdatedConsumer : BasePriceUpdatedConsumer, IConsumer<StockPriceUpdated>
    {
        public override FinancalAssetType AssetType => FinancalAssetType.Stock;

        public StockPriceUpdatedConsumer(ITrackerCacheService cache)
            : base(cache) { }

        public async Task Consume(ConsumeContext<StockPriceUpdated> context)
        {
            var instance = context.Message;

            var newCacheItem = new FinancialAssetItem
            {
                Symbol = instance.Symbol,
                Price = instance.Price,
                Type = AssetType
            };

            await HandlePriceUpdate(newCacheItem);
        }
    }
}
