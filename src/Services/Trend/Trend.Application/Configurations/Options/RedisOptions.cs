namespace Trend.Application.Configurations.Options
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string InstanceName { get; set; } = default!;
        public int RememberTime { get; set; }
    }
}
