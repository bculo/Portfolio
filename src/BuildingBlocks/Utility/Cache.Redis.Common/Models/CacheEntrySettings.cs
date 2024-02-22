namespace Cache.Redis.Common.Models;

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