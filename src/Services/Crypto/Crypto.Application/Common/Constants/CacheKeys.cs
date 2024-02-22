using Cache.Redis.Common;
using Cache.Redis.Common.Models;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Common.Constants;

public static class CacheKeys
{
    private const string CRYPTO_ITEM_PREFIX = "cryptoitem";
    
    public static string CryptoItemKey(string symbol) => $"{CRYPTO_ITEM_PREFIX}:{symbol.ToLower()}";

    public static Action<FusionCacheEntryOptions> CryptoItemKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
}