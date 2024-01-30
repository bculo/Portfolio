using Cache.Redis.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Stock.Application.Common.Constants;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Infrastructure.Html;
using Stock.Infrastructure.Localization;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Persistence.Repositories;
using Stock.Infrastructure.Price;
using Time.Abstract.Contracts;
using Time.Common;

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

            services.AddDbContext<StockDbContext>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockPriceRepository, StockPriceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILocale, LocaleService>();

            var instanceName = configuration["RedisOptions:InstanceName"];
            var connection = configuration["RedisOptions:ConnectionString"];
            services.AddRedisCacheService(connection!, instanceName!);
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureMarketWatchClient(services, configuration);
        }

        private static void ConfigureMarketWatchClient(IServiceCollection services, IConfiguration configuration)
        {
            string baseAddress = configuration["MarketWatchOptions:BaseUrl"] ?? throw new ArgumentNullException();
            int retryNumber = configuration.GetValue<int>("MarketWatchOptions:RetryNumber");
            int timeout = configuration.GetValue<int>("MarketWatchOptions:Timeout");

            services.AddHttpClient(HttpClientNames.MARKET_WATCH, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(
                Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber)));

            services.AddTransient<IStockPriceClient, MarketWatchStockPriceClient>();
        }
    }
}
