using System.Text.Json;
using System.Text.Json.Serialization;
using Cache.Redis.Common;
using Cache.Redis.Common.Models;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Common.Constants;

public static class CacheKeys
{
    public const string EVICT_ON_PRICE_REFRESH = "refresh";
    
    public static string SingleItemKey(string symbol) => $"single:{symbol.ToLower()}";
    public static string MostPopularKey(int limitNum) => $"popular:{EVICT_ON_PRICE_REFRESH}:{limitNum}";
    public static string FetchPageKey(FetchPageQuery query) => $"page:{EVICT_ON_PRICE_REFRESH}:{JsonSerializer.Serialize(query)}";

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
    
    public static Action<FusionCacheEntryOptions> FetchPageKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(2),
            TimeSpan.FromMinutes(3),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
}