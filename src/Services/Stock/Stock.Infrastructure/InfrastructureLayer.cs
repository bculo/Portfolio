using System.Globalization;
using Cache.Redis.Common;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
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
using StackExchange.Redis;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Options;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Infrastructure.Extensions;
using Stock.Infrastructure.Html;
using Stock.Infrastructure.Localization;
using Stock.Infrastructure.Persistence;
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
            
            AddClients(services, configuration, isDevEnv);
            AddCache(services, configuration);
            AddLocalization(services, configuration);
            AddHangfire(services, configuration);
            
            services.AddDbContext<StockDbContext>();
            services.AddScoped<IDataSourceProvider, DataSourceProvider>();
            services.AddScoped<IEntityManagerRepository, EntityManagerRepository>();
            services.AddScoped<ILocale, LocaleService>();
            services.AddScoped<IHtmlParserFactory, HtmlParserFactory>();
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
            services.Configure<BatchUpdateOptions>(configuration.GetSection("BatchUpdateOptions"));
            
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
            services.AddHttpClient();
            
            var baseAddress = configuration["MarketWatchOptions:BaseUrl"] 
                              ?? throw new ArgumentNullException();
            var retryNumber = configuration.GetValue<int>("MarketWatchOptions:RetryNumber");
            var timeout = configuration.GetValue<int>("MarketWatchOptions:Timeout");
            
            services.AddTransient<ProxyHttpMessageHandler>();
            services.AddHttpClient(HttpClientNames.MarketWatch, client =>
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

            //services.AddTransient<IStockPriceClient, MarketWatchStockPriceClient>();
            services.AddTransient<IStockPriceClient, FakePriceClient>();
        }
        
        private static void AddCache(IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var redisInstanceName = configuration["RedisOptions:InstanceName"];
            
            services.AddRedisFusionCacheService(redisConnectionString!, redisInstanceName!);
            services.AddRedisOutputCache(redisConnectionString!, redisInstanceName!);
        }
        
        public static void AddMessageQueue(IServiceCollection services, 
            IConfiguration configuration,
            Action<IBusRegistrationConfigurator>? registerClient = null)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "stock", false));
                
                x.AddEntityFrameworkOutbox<StockDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                });
                
                registerClient?.Invoke(x);

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    if (registerClient != null)
                    {
                        config.ConfigureEndpoints(context);
                    }
                });
            });
        }
    }
}
