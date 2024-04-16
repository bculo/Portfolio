namespace Cache.Redis.Common.Models;

public record CacheEntrySettings
{
    public TimeSpan Duration { get; }
    public TimeSpan? FailOver { get; }
    public FactoryTimeoutOption? FactoryTimeout { get; }
    public bool UseEagerRefresh { get; }


    public CacheEntrySettings(TimeSpan duration, 
        TimeSpan? failOver, 
        FactoryTimeoutOption? factoryTimeout,
        bool useEagerRefresh = false)
    {
        if (failOver.HasValue && duration >= failOver.Value)
        {
            throw new ArgumentException($"Fail over must be greater than duration");
        }

        Duration = duration;
        FailOver = failOver;
        FactoryTimeout = factoryTimeout;
        UseEagerRefresh = useEagerRefresh;
    } 
}