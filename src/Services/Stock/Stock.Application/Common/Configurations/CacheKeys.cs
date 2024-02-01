using Stock.Application.Common.Extensions;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Common.Configurations;



public static class CacheKeys
{
    public static string StockItemKey(int id) => $"stock:{id}";
    
    public static string StockItemKey(string symbol) => $"stock:{symbol}";

    public static Action<FusionCacheEntryOptions> StockItemKeyOptions(CacheEntrySettings? settings = default)
    {
        var keySettings = settings ?? new CacheEntrySettings(TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(20),
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
        if (hardTimeout < softTimeout)
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
        if (failOver.HasValue && duration >= failOver.Value)
        {
            throw new ArgumentException($"Fail over must be greater than duration");
        }

        Duration = duration;
        FailOver = failOver;
        FactoryTimeout = factoryTimeout;
    } 
}