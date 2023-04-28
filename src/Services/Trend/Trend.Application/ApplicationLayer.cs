using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using System.Diagnostics;
using Time.Common;
using Time.Common.Contracts;
using Trend.Application.Clients;
using Trend.Application.Configurations.Persistence;
using Trend.Application.Interfaces;
using Trend.Application.Options;
using Trend.Application.Repositories;
using Trend.Application.Services;
using Trend.Application.Utils.Persistence;
using Trend.Domain.Interfaces;

namespace Trend.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ISyncService, SyncService>();
            services.AddScoped<ISearchWordService, SearchWordService>();
            services.AddScoped(typeof(ILanguageService<>), typeof(LanguageService<>));

            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(ApplicationLayer).Assembly));
        }

        public static void AddCache(IConfiguration configuration, IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });

            services.AddScoped<ICacheService, CacheService>();
        }

        public static void AddPersistence(IConfiguration configuration, IServiceCollection services)
        {
            MongoConfiguration.Configure();

            services.Configure<MongoOptions>(configuration.GetSection("MongoOptions"));
            var mongoDbOptions = configuration.GetSection("MongoOptions").Get<MongoOptions>();
            services.AddSingleton<IMongoClient>(c => TrendMongoUtils.CreateMongoClient(mongoDbOptions));
            services.AddScoped(c => c.GetRequiredService<IMongoClient>().StartSession());
            services.AddScoped<ITransaction, MongoTransactionService>();

            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddScoped<ISyncStatusRepository, SyncStatusRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ISearchWordRepository, SearchWordRepository>();
        }

        public static void AddLogger(IHostBuilder host)
        {
            host.UseSerilog((ctx, cl) =>
            {
                cl.ReadFrom.Configuration(ctx.Configuration);

                cl.Enrich.FromLogContext();
                cl.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);
                cl.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName);

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
            services.AddScoped<IGoogleSyncService, GoogleSyncService>();
            services.AddScoped<IGoogleSearchClient, GoogleSearchClient>();
        }
    }
}
