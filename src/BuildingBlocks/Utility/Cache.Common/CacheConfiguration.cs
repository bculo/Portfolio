using Cache.Abstract.Contracts;
using Cache.Common.Redis;
using Cache.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cache.Common
{
    public static class CacheConfiguration
    {
        public static IConnectionMultiplexer AddRedis(IServiceCollection services, 
            IConfiguration configuration,
            IConnectionMultiplexer multiplexer = null)
        {
            services.AddScoped<ICacheService, CacheService>();
            services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];

                if (multiplexer is not null)
                {
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer);
                }
            });

            return multiplexer;
        }

        public static IConnectionMultiplexer AddConnectionMultiplexer(IServiceCollection services,
            string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            var multiplexer = ConnectionMultiplexer.Connect(connectionString) as IConnectionMultiplexer;
            services.AddSingleton(multiplexer);
            return multiplexer;
        }
    }
}
