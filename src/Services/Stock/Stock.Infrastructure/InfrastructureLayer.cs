using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Stock.Application.Common.Constants;
using Stock.Application.Interfaces;
using Stock.Infrastructure.Clients;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Persistence.Repositories;
using Stock.Infrastructure.Services;
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
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureMarketWatchClient(services, configuration);
        }

        private static void ConfigureMarketWatchClient(IServiceCollection services, IConfiguration configuration)
        {
            string baseAddress = configuration["MarketWatchOptions:BaseUrl"];
            int retryNumber = configuration.GetValue<int>("MarketWatchOptions:RetryNumber");
            int timeout = configuration.GetValue<int>("MarketWatchOptions:Timeout");

            services.AddHttpClient(HttpClientNames.MARKET_WATCH, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
            {
                return policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber));
            });

            services.AddTransient<IStockPriceClient, MarketWatchStockPriceClient>();
        }
    }
}
