using Cache.Redis.Common;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Stock.Application.Common.Configurations;
using Stock.Application.Interfaces.Expressions;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Infrastructure.Expressions;
using Stock.Infrastructure.Html;
using Stock.Infrastructure.Localization;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Persistence.Repositories;
using Stock.Infrastructure.Persistence.Repositories.Read;
using Stock.Infrastructure.Price;
using Time.Abstract.Contracts;
using Time.Common;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Stock.Infrastructure
{
    public static class InfrastructureLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDateTimeProvider, UtcDateTimeService>();
            services.AddTransient<IHtmlParser, HtmlParserService>(opt =>
            {
                var logger = opt.GetRequiredService<ILogger<HtmlParserService>>();
                return new HtmlParserService(logger, null);
            });

            AddClients(services, configuration);
            AddCache(services, configuration);

            services.AddDbContext<StockDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockPriceRepository, StockPriceRepository>();
            services.AddScoped<IStockWithPriceTagReadRepository, StockWithPriceTagReadRepository>();
            
            services.AddScoped<ILocale, LocaleService>();
            services.AddScoped(typeof(IExpressionBuilder<>), typeof(ExpressionBuilder<>));
            services.AddScoped<IExpressionBuilderFactory, ExpressionBuilderFactory>();
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureMarketWatchClient(services, configuration);
        }
        
        public static void AddCache(IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var redisInstanceName = configuration["RedisOptions:InstanceName"];
            
            services.AddFusionCache()
                .WithDefaultEntryOptions(opt =>
                {
                    opt.FactorySoftTimeout = TimeSpan.FromMilliseconds(200);
                    opt.FactoryHardTimeout = TimeSpan.FromMilliseconds(2000);

                    opt.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1);
                    opt.DistributedCacheHardTimeout = TimeSpan.FromSeconds(2);
                    opt.AllowBackgroundDistributedCacheOperations = true;
                })
                .WithSerializer(new FusionCacheSystemTextJsonSerializer())
                .WithDistributedCache(
                    new RedisCache(new RedisCacheOptions
                    {
                        Configuration = redisConnectionString, 
                        InstanceName = redisInstanceName
                    })
                );
            
            services.AddRedisOutputCache(redisConnectionString!, redisInstanceName!);
        }

        private static void ConfigureMarketWatchClient(IServiceCollection services, IConfiguration configuration)
        {
            var baseAddress = configuration["MarketWatchOptions:BaseUrl"] ?? throw new ArgumentNullException();
            var retryNumber = configuration.GetValue<int>("MarketWatchOptions:RetryNumber");
            var timeout = configuration.GetValue<int>("MarketWatchOptions:Timeout");

            services.AddHttpClient(HttpClientNames.MARKET_WATCH, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => 
                policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber)));

            services.AddTransient<IStockPriceClient, MarketWatchStockPriceClient>();
        }
    }
}
