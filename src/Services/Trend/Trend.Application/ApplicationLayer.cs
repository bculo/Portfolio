using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using System.Diagnostics;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Time.Abstract.Contracts;
using Time.Common;
using Trend.Application.Configurations.Initialization;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Jobs;
using Trend.Application.Repositories;
using Trend.Application.Services;
using Trend.Application.Utils;

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
            services.AddScoped(typeof(ILanguageService<>), typeof(LanguageService<>));

            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        }

        public static void AddPersistence(IConfiguration configuration, IServiceCollection services)
        {
            MongoConfiguration.Configure();

            services.Configure<MongoOptions>(configuration.GetSection("MongoOptions"));
            var mongoDbOptions = configuration.GetSection("MongoOptions").Get<MongoOptions>();
            services.AddSingleton<IMongoClient>(_ => TrendMongoUtils.CreateMongoClient(mongoDbOptions));
            services.AddScoped(c => c.GetRequiredService<IMongoClient>().StartSession());
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

                if(!ctx.Configuration.GetValue<bool>("SerilogMongo:UseLogger"))
                {
                    return;
                }

                cl.WriteTo.MongoDBBson(cfg =>
                {
                    var identity = new MongoInternalIdentity(
                        ctx.Configuration["SerilogMongo:AuthDatabase"], 
                        ctx.Configuration["SerilogMongo:UserName"]);
                    var evidence = new PasswordEvidence(ctx.Configuration["SerilogMongo:Password"]);

                    var mongoDbSettings = new MongoClientSettings
                    {
                        UseTls = false,
                        Credential = new MongoCredential(ctx.Configuration["SerilogMongo:AuthMechanisam"], identity, evidence),
                        Server = new MongoServerAddress(
                            ctx.Configuration["SerilogMongo:Host"], 
                            ctx.Configuration.GetValue<int>("SerilogMongo:Port")),
                    };

                    var mongoDbInstance = new MongoClient(mongoDbSettings).GetDatabase(ctx.Configuration["SerilogMongo:Database"]);

                    cfg.SetMongoDatabase(mongoDbInstance);
                });
            });

            Serilog.Debugging.SelfLog.Enable(msg => {
                Debug.WriteLine(msg);
            });
        }

        public static void AddClients(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHttpClient();
            services.Configure<GoogleSearchOptions>(configuration.GetSection("GoogleSearchOptions"));
            services.AddScoped<ISearchEngine, GoogleSearchEngine>();
            services.AddScoped<IGoogleSearchClient, GoogleSearchClient>();
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
                    });
            });

            if (!addServers)
            {
                return;
            }
            
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
    }
}
