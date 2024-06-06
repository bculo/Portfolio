namespace Crypto.Application.Common.Options
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; init; } = default!;
        public string InstanceName { get; init; } = default!;
        public int ExpirationTime { get; init; }
    }
}
