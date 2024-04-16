using Cache.Redis.Common.Models;
using ZiggyCreatures.Caching.Fusion;

namespace Cache.Redis.Common;

public static class FusionCacheEntryOptionsExtensions
{
    public static FusionCacheEntryOptions ApplyOptions(
        this FusionCacheEntryOptions options,
        int defaultDurationMin = 30,
        CacheEntrySettings? settings = default)
    {
        if (settings is null)
        {
            return options.SetDuration(TimeSpan.FromMinutes(defaultDurationMin));
        }
        
        return options.SetDuration(settings.Duration)
            .ApplyFailSafe(settings.FailOver)
            .ApplyFactoryTimeout(settings.FactoryTimeout)
            .ApplyEagerRefresh(settings.UseEagerRefresh);
    }
    
    private static FusionCacheEntryOptions ApplyFailSafe(
        this FusionCacheEntryOptions entryOptions, 
        TimeSpan? timeSpan = default)
    {
        if (!timeSpan.HasValue)
        {
            return entryOptions;
        }

        return entryOptions.SetFailSafe(true, timeSpan);
    }
    
    private static FusionCacheEntryOptions ApplyEagerRefresh(
        this FusionCacheEntryOptions entryOptions, 
        bool useEagerRefresh)
    {
        if (!useEagerRefresh)
        {
            return entryOptions;
        }

        return entryOptions.SetEagerRefresh(.9f);
    }
    
    private static FusionCacheEntryOptions ApplyFactoryTimeout(
        this FusionCacheEntryOptions entryOptions, 
        FactoryTimeoutOption? option = default)
    {
        if (option is null)
        {
            return entryOptions;
        }

        return entryOptions.SetFactoryTimeouts(option.SoftTimeout, option.HardTimeout);
    }
}