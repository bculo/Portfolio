namespace Cache.Common.Redis
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int ExpirationTime { get; set; }
    }
}
