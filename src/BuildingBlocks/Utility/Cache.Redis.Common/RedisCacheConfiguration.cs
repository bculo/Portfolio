using Cache.Abstract.Contracts;
using Cache.Redis.Common.Redis;
using Cache.Redis.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cache.Redis.Common
{
    public static class RedisCacheConfiguration
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

        public static IConnectionMultiplexer GetRedisConnectionMultiplexer(this IServiceCollection services,
            string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            var multiplexer = ConnectionMultiplexer.Connect(connectionString) as IConnectionMultiplexer;
            return multiplexer;
        }

        public static void AddRedisOutputCache(this IServiceCollection services, 
            string connectionString, 
            string instanceName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(instanceName);
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            
            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
            });

            services.AddOutputCache();
        }
    }
}
