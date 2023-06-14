using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Application.Common.Models;
using Tracker.Application.Common.Utilities;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.Infrastructure.Consumers
{
    public abstract class BasePriceUpdatedConsumer
    {
        protected readonly ITrackerCacheService _cache;

        protected BasePriceUpdatedConsumer(ITrackerCacheService cache)
        {
            _cache = cache;
        }

        public abstract FinancalAssetType AssetType { get; }

        public virtual async Task HandlePriceUpdate(FinancialAssetItem item)
        {
            var key = CacheKeyUtilities.CombineKey(AssetType, item.Symbol);
            await _cache.Add(key, item);
        }
    }
}
