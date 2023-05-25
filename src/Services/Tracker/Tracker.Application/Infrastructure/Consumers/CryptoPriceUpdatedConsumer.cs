using Events.Common.Crypto;
using MassTransit;
using Tracker.Application.Constants;
using Tracker.Application.Interfaces;
using Tracker.Application.Models;
using Tracker.Application.Utilities;
using Tracker.Core.Enums;

namespace Tracker.Application.Infrastructure.Consumers;

public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
{
    private readonly ITrackerCacheService _cache;

    public CryptoPriceUpdatedConsumer(ITrackerCacheService cache)
    {
        _cache = cache;
    }

    public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
    {
        var instance = context.Message;
        
        var newCacheItem = new FinancialAssetItem
        {
            Symbol = instance.Symbol,
            Price = instance.Price,
            Type = FinancalAssetType.Crypto
        };
        
        var key = CacheKeyUtilities.CombineKey(FinancalAssetType.Crypto, instance.Symbol);
        await _cache.Add(key, newCacheItem);
    }
}