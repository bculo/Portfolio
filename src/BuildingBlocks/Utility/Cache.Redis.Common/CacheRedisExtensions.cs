using Cache.Redis.Common.Interfaces;
using Cache.Redis.Common.Options;
using Cache.Redis.Common.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Cache.Redis.Common
{
    public static class CacheRedisExtensions
    {
        public static void AddRedisCacheService(this IServiceCollection services, 
            string connectionString, 
            string instanceName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(instanceName);
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddOptions<RedisOptions>()
                .Configure(opt =>
                {
                    opt.InstanceName = instanceName;
                    opt.ConnectionString = connectionString;
                });
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
            });
        }
        
        public static void AddRedisFusionCacheService(this IServiceCollection services, 
            string connectionString, 
            string instanceName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(instanceName);
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            
            services.AddRedisConnectionMultiplexer(connectionString);
            
            services.AddFusionCache()
                .WithDefaultEntryOptions(opt =>
                {
                    opt.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1);
                    opt.DistributedCacheHardTimeout = TimeSpan.FromSeconds(2);
                    opt.AllowBackgroundDistributedCacheOperations = true;
                })
                .WithSerializer(new FusionCacheSystemTextJsonSerializer())
                .WithDistributedCache(
                    new RedisCache(new RedisCacheOptions
                    {
                        Configuration = connectionString, 
                        InstanceName = instanceName
                    })
                );
        }

        public static void AddRedisConnectionMultiplexer(this IServiceCollection services,
            string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            var multiplexer = ConnectionMultiplexer.Connect(connectionString) as IConnectionMultiplexer;
            services.TryAddSingleton(multiplexer);
        }

        public static void AddRedisOutputCache(this IServiceCollection services, 
            string connectionString, 
            string instanceName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(instanceName);
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

            services.AddRedisConnectionMultiplexer(connectionString);
            
            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope(); 
            var multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            
            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
                options.ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer);
            });

            services.AddOutputCache();
        }
    }
}
