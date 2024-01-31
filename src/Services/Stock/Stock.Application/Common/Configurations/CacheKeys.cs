using Stock.Application.Common.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Common.Configurations;



public static class CacheKeys
{
    public static string StockItemKey(int id) => $"stock:{id}";

    public static Action<FusionCacheEntryOptions> StockItemKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(60),
            new FactoryTimeoutOption(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)));

        return (opt) => opt.ApplyOptions(settings: keySettings);
    }
}


public record FactoryTimeoutOption
{
    public TimeSpan SoftTimeout { get; }
    public TimeSpan HardTimeout { get; }
    
    public FactoryTimeoutOption(TimeSpan softTimeout, TimeSpan hardTimeout)
    {
        if (hardTimeout.Milliseconds < softTimeout.Milliseconds)
        {
            throw new ArgumentException($"Hard timout must be greater than soft timeout");
        }
        
        SoftTimeout = softTimeout;
        HardTimeout = hardTimeout;
    }   
}

public record CacheEntrySettings
{
    public TimeSpan Duration { get; }
    public TimeSpan? FailOver { get; }
    public FactoryTimeoutOption? FactoryTimeout { get; }


    public CacheEntrySettings(TimeSpan duration, TimeSpan? failOver, FactoryTimeoutOption? factoryTimeout)
    {
        if (failOver.HasValue && duration.Milliseconds >= failOver.Value.Milliseconds)
        {
            throw new ArgumentException($"Duration must be greater than fail over duration");
        }

        Duration = duration;
        FailOver = failOver;
        FactoryTimeout = factoryTimeout;
    } 
}