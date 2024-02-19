using Stock.Application.Common.Constants;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Common.Extensions;

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
            .ApplyFactoryTimeout(settings.FactoryTimeout);
    }
    
    public static FusionCacheEntryOptions ApplyFailSafe(
        this FusionCacheEntryOptions entryOptions, 
        TimeSpan? timeSpan = default)
    {
        if (!timeSpan.HasValue)
        {
            return entryOptions;
        }

        return entryOptions.SetFailSafe(true, timeSpan);
    }
    
    public static FusionCacheEntryOptions ApplyFactoryTimeout(
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