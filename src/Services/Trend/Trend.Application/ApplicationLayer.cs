using System.Diagnostics;
using Azure.Storage.Blobs;
using Cache.Redis.Common;
using FluentValidation;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Debugging;
using StackExchange.Redis;
using Time.Abstract.Contracts;
using Time.Common;
using Trend.Application.Configurations.Constants;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Repositories;
using Trend.Application.Repositories.Initialization;
using Trend.Application.Services;
using Trend.Application.Utils;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;
using ITransaction = Trend.Application.Interfaces.ITransaction;

namespace Trend.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleServiceEnumerable, ArticleServiceEnumerable>();
            services.AddScoped<ISyncService, SyncService>();
            services.AddScoped<ISearchWordService, SearchWordService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped(typeof(ILanguageService<>), typeof(LanguageService<>));

            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
            
            services.AddScoped<IImageService, MagickImageService>();
            
            services.AddSingleton<IBlobStorage, BlobStorage>((provider) =>
            {
                var options = provider.GetRequiredService<IOptions<BlobStorageOptions>>();
                var logger = provider.GetRequiredService<ILogger<BlobStorage>>();
                var azureBlobServiceClient = new BlobServiceClient(options.Value.ConnectionString);
                var storage = new BlobStorage(azureBlobServiceClient, options, logger);
                storage.InitializeContext(options.Value.TrendContainerName, true);
                return storage;
            });
            
            services.Configure<BlobStorageOptions>(configuration.GetSection("BlobStorageOptions"));
            
            services.AddHttpClient();
            services.Configure<GoogleSearchOptions>(configuration.GetSection("GoogleSearchOptions"));
            
            services.AddHttpClient(HttpClientNames.GOOGLE_CLIENT)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = 
                        new Uri(configuration["GoogleSearchOptions:Uri"] ?? throw new ArgumentNullException());
                })
                .AddPolicyHandler(
                    HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            Backoff.DecorrelatedJitterBackoffV2(
                                TimeSpan.FromSeconds(0.5),
                                3)));

            services.AddScoped<IGoogleSearchClient, GoogleSearchClient>();
            services.AddScoped<ISearchEngine, GoogleSearchEngine>();
            
            MongoConfiguration.Configure();

            services.Configure<MongoOptions>(configuration.GetSection("MongoOptions"));
            var mongoDbOptions = configuration.GetSection("MongoOptions").Get<MongoOptions>();
            services.AddSingleton<IMongoClient>(_ => TrendMongoUtils.CreateMongoClient(mongoDbOptions));
            services.AddScoped(c => c.GetRequiredService<IMongoClient>().StartSession());
            services.AddSingleton<IMongoDatabase>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<MongoOptions>>();
                return provider.GetRequiredService<IMongoClient>().GetDatabase(options.Value.DatabaseName);
            });
            services.AddScoped<ITransaction, MongoTransactionService>();

            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddScoped<ISyncStatusRepository, SyncStatusRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ISearchWordRepository, SearchWordRepository>();
            
            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
        }
        
        public static void AddLogger(IHostBuilder host)
        {
            host.UseSerilog((ctx, cl) =>
            {
                cl.ReadFrom.Configuration(ctx.Configuration);
                cl.Enrich.FromLogContext();
                cl.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);
                cl.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName);
            });

            SelfLog.Enable(msg => {
                Debug.WriteLine(msg);
            });
        }
        
        public static void ConfigureHangfire(IConfiguration configuration, 
            IServiceCollection services, 
            bool addServers = false)
        {
            services.AddHangfire((provider, config) =>
            {
                var client = provider.GetRequiredService<IMongoClient>();
                var databaseName = configuration["MongoOptions:DatabaseName"];
                
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMongoStorage(client, databaseName, new MongoStorageOptions
                    {
                        MigrationOptions = new MongoMigrationOptions
                        {
                            MigrationStrategy = new MigrateMongoMigrationStrategy(),
                            BackupStrategy = new CollectionMongoBackupStrategy()
                        },

                        Prefix = "hangfire",
                        CheckConnection = true,
                        InvisibilityTimeout = TimeSpan.FromMinutes(3),
                        DistributedLockLifetime = TimeSpan.FromMinutes(1)
                    });
            });

            if (!addServers) return;

            services.AddHangfireServer(opt =>
            {
                opt.ShutdownTimeout = TimeSpan.FromSeconds(5);
            });
            
            services.AddScoped<ISyncJob, SyncJob>();
            
            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            
            manager.AddOrUpdate<ISyncJob>(
                configuration["Jobs:SyncJob:Name"],
                s => s.Work(default),
                configuration["Jobs:SyncJob:Cron"]);
        }

        public static void ConfigureCache(IConfiguration configuration, IServiceCollection services)
        {
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var redisInstanceName = configuration["RedisOptions:InstanceName"];
            
            services.AddFusionCache()
                .WithDefaultEntryOptions(opt =>
                {
                    opt.IsFailSafeEnabled = false;
                    opt.FailSafeMaxDuration = TimeSpan.FromHours(3);
                    opt.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);

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
            
            services.AddRedisConnectionMultiplexer(redisConnectionString!);
            services.AddRedisOutputCache(redisConnectionString!, redisInstanceName!);
        }
        
        public static void AddOpenTelemetry(IConfiguration config, 
            IServiceCollection services,
            string appName)
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName: appName);

            var exporterUri = new Uri(config["OpenTelemetry:OtlpExporter"] ?? throw new ArgumentNullException());
           
            services.AddOpenTelemetry()
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .SetErrorStatusOnException()
                    .AddHttpClientInstrumentation()
                    .AddMongoDBInstrumentation()
                    .AddRedisInstrumentation()
                    .ConfigureRedisInstrumentation((provider, instrumentation) =>
                    {
                        var multiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
                        instrumentation.AddConnection(multiplexer);
                    })
                    .SetResourceBuilder(resourceBuilder)
                    .AddSource(Telemetry.Trend.Name)
                    .AddSource("MassTransit")
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = exporterUri;
                    })
                );
        }
    }
}
