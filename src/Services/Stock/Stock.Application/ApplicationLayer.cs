using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Stock.Application.Constants;
using Stock.Application.Infrastructure.Clients;
using Stock.Application.Infrastructure.Services;
using Stock.Application.Interfaces;
using Time.Common;
using Time.Common.Contracts;

namespace Stock.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHtmlParser, HtmlParserService>(opt =>
            {
                var logger = opt.GetRequiredService<ILogger<HtmlParserService>>();
                return new HtmlParserService(logger, null);
            });

            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
        }

        public static void AddClients(IServiceCollection services, IConfiguration configuration)
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

            services.AddScoped<IStockPriceClient, MarketWatchStockPriceClient>();
        }
    }
}
