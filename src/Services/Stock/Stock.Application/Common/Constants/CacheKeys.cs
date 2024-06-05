using Cache.Redis.Common;
using Cache.Redis.Common.Models;
using Stock.Application.Common.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Common.Constants;

public static class CacheKeys
{
    private const string Prefix = "stock";
    
    public static string StockItemKey(int id) => $"{Prefix}:{id}";
    
    public static string StockItemKey(string symbol) => $"{Prefix}:{symbol}";

    public static Action<FusionCacheEntryOptions> StockItemKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
}