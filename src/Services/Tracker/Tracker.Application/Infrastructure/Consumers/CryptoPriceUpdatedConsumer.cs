using Events.Common.Crypto;
using MassTransit;
using Tracker.Application.Constants;
using Tracker.Application.Interfaces;
using Tracker.Application.Utilities;

namespace Tracker.Application.Infrastructure.Consumers;

public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
{
    private readonly ITrendCacheService _cache;

    public CryptoPriceUpdatedConsumer(ITrendCacheService cache)
    {
        _cache = cache;
    }

    public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
    {
        var instance = context.Message;
        var key = CacheKeyUtilities.CombineKey(CacheConstants.CRYPTO_PREFIX, instance.Symbol);
        await _cache.Add(key, instance);
    }
}