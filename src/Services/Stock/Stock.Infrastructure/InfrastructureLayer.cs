using System.Globalization;
using Cache.Redis.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Queryable.Common;
using StackExchange.Redis;
using Stock.Application.Common.Configurations;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Infrastructure.Common.Extensions;
using Stock.Infrastructure.Html;
using Stock.Infrastructure.Localization;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Persistence.Repositories;
using Stock.Infrastructure.Persistence.Repositories.Read;
using Stock.Infrastructure.Price;
using Time.Common;

namespace Stock.Infrastructure
{
    public static class InfrastructureLayer
    {
        public static void AddServices(IServiceCollection services, 
            IConfiguration configuration, 
            bool isDevEnv = true)
        {
            services.AddUtcTimeProvider();
            services.AddTransient<IHtmlParser, HtmlParserService>(opt =>
            {
                var logger = opt.GetRequiredService<ILogger<HtmlParserService>>();
                return new HtmlParserService(logger, null);
            });

            AddClients(services, configuration, isDevEnv);
            AddCache(services, configuration);
            AddLocalization(services, configuration);
            AddHangfire(services, configuration);
            
            services.AddSingleton<QueryInterceptor>();
            services.AddDbContext<StockDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockPriceRepository, StockPriceRepository>();
            services.AddScoped<IStockWithPriceTagReadRepository, StockWithPriceTagReadRepository>();
            
            services.AddScoped<ILocale, LocaleService>();
        }

        public static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration, string appName)
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName: appName);

            var exporterUri = new Uri(configuration["OpenTelemetry:OtlpExporter"] ?? throw new ArgumentNullException());
           
            services.AddOpenTelemetry()
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .SetErrorStatusOnException()
                    .AddHttpClientInstrumentation()
                    .AddRedisInstrumentation()
                    .ConfigureRedisInstrumentation((provider, instrumentation) =>
                    {
                        var multiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
                        instrumentation.AddConnection(multiplexer);
                    })
                    .AddNpgsql()
                    .SetResourceBuilder(resourceBuilder)
                    .AddSource("MassTransit")
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = exporterUri;
                    })
                );
        }

        private static void AddHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UsePostgreSqlStorage(opt =>
                {
                    opt.UseNpgsqlConnection(configuration.GetConnectionString("StockDatabase"));
                });
            });
        }

        private static void AddLocalization(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options => 
            {
                var supportedCultures = new List<CultureInfo> 
                {
                    new("en-US"),
                    new("hr-HR")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration, bool isDevEnv)
        {
            var baseAddress = configuration["MarketWatchOptions:BaseUrl"] 
                              ?? throw new ArgumentNullException();
            var retryNumber = configuration.GetValue<int>("MarketWatchOptions:RetryNumber");
            var timeout = configuration.GetValue<int>("MarketWatchOptions:Timeout");

            services.AddHttpClient(HttpClientNames.MARKET_WATCH, client =>
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.Timeout = TimeSpan.FromSeconds(timeout);
                })
                .ApplyHttpMessageHandler<ProxyHttpMessageHandler>(!isDevEnv)  
                .AddPolicyHandler(
                    HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            Backoff.DecorrelatedJitterBackoffV2(
                                TimeSpan.FromSeconds(0.5),
                                retryNumber)));

            services.AddTransient<IStockPriceClient, MarketWatchStockPriceClient>();
        }
        
        private static void AddCache(IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var redisInstanceName = configuration["RedisOptions:InstanceName"];
            
            services.AddRedisFusionCacheService(redisConnectionString!, redisInstanceName!);
            services.AddRedisOutputCache(redisConnectionString!, redisInstanceName!);
        }
    }
}
