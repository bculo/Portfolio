namespace Cache.Redis.Common.Models;

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