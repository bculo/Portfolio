using Cache.Abstract.Contracts;
using Cache.Common.Redis;
using Cache.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cache.Common
{
    public static class CacheConfiguration
    {
        public static void AddRedis(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICacheService, CacheService>();
            services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });
        }
    }
}
