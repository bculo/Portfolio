using Crypto.Application.Common.Constants;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Infrastructure.Consumers;

public class EvictRedisListRequestConsumer : IConsumer<EvictRedisListRequest>
{
    private readonly IConnectionMultiplexer _multiplexer;
    private readonly IConfiguration _config;
    private readonly IFusionCache _fusionCache;

    public EvictRedisListRequestConsumer(IConnectionMultiplexer multiplexer, 
        IConfiguration config, 
        IFusionCache fusionCache)
    {
        _multiplexer = multiplexer;
        _config = config;
        _fusionCache = fusionCache;
    }

    public async Task Consume(ConsumeContext<EvictRedisListRequest> context)
    {
        await foreach (var key in GetRedisKeysForEviction())
        {
            await RemoveFromCache(key);
        }
    }

    private async Task RemoveFromCache(string key)
    {
        await _fusionCache.RemoveAsync(key);
    }

    private async IAsyncEnumerable<string> GetRedisKeysForEviction()
    {
        var server = _multiplexer.GetServer(_config["RedisOptions:ConnectionString"]);
        await foreach (var item in server.KeysAsync(pattern: GetScanPattern()))
        {
            yield return RemoveRedisInstanceNamePrefix(item);
        }
    }

    private string GetScanPattern()
    {
        return $"*{CacheKeys.EVICT_ON_PRICE_REFRESH}*";
    }
    
    private string RemoveRedisInstanceNamePrefix(RedisKey key)
    {
        var splitter = ":";
        var keyParts = key.ToString().Split(splitter).ToList();
        keyParts.RemoveAt(0);
        return string.Join(splitter, keyParts);
    }
}