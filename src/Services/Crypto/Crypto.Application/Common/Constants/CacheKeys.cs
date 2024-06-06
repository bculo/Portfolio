using System.Text.Json;
using Cache.Redis.Common;
using Cache.Redis.Common.Models;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Common.Constants;

public static class CacheKeys
{
    public const string EvictOnPriceRefresh = "refresh";
    
    public static string SingleItemKey(string symbol) => $"single:{symbol.ToLower()}";
    public static string MostPopularKey(int limitNum) => $"popular:{EvictOnPriceRefresh}:{limitNum}";
    public static string FetchCryptoPageKey(FetchPageQuery query) 
        => $"page:{EvictOnPriceRefresh}:{JsonSerializer.Serialize(query)}";
    public static string FetchPriceHistoryKey(FetchPriceHistoryQuery query)
        => $"price:{EvictOnPriceRefresh}:{JsonSerializer.Serialize(query)}";
    
    public static Action<FusionCacheEntryOptions> SingleItemKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(
            TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
    
    public static Action<FusionCacheEntryOptions> MostPopularKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(
            TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
    
    public static Action<FusionCacheEntryOptions> FetchCryptoPageKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(
            TimeSpan.FromMinutes(2),
            TimeSpan.FromMinutes(3),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
    
    public static Action<FusionCacheEntryOptions> FetchPriceHistoryKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(
            TimeSpan.FromMinutes(2),
            TimeSpan.FromMinutes(3),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
}