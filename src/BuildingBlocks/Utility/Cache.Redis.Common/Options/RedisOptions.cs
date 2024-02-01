namespace Cache.Redis.Common.Options
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string InstanceName { get; set; } = default!;
    }
}
