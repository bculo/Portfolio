using Cache.Redis.Common;
using Cache.Redis.Common.Models;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Common.Constants;

public static class CacheKeys
{
    public static string SingleItemKey(string symbol) => $"single:{symbol.ToLower()}";
    public static string MostPopularKey() => $"popular";

    public static Action<FusionCacheEntryOptions> SingleItemKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
    
    public static Action<FusionCacheEntryOptions> MostPopularKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
}