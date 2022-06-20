using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common;
using Time.Common.Contracts;
using Trend.Application.Background;
using Trend.Application.Clients;
using Trend.Application.Configurations.Persistence;
using Trend.Application.Interfaces;
using Trend.Application.Options;
using Trend.Application.Repositories;
using Trend.Application.Services;
using Trend.Domain.Interfaces;

namespace Trend.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<GoogleSearchOptions>(configuration.GetSection("GoogleSearchOptions"));
            services.Configure<MongoOptions>(configuration.GetSection("MongoOptions"));
            services.Configure<SyncBackgroundServiceOptions>(configuration.GetSection("SyncBackgroundServiceOptions"));

            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddScoped<ISyncStatusRepository, SyncStatusRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ISearchWordRepository, SearchWordRepository>();

            MongoConfiguration.Configure();
            //builder.Services.AddScoped<IMongoContext, MongoContext>();
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IGoogleSyncService, GoogleSyncService>();
            services.AddScoped<IGoogleSearchClient, GoogleSearchClient>();
            services.AddScoped<IDateTime, LocalDateTimeService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ISyncService, SyncService>();
            services.AddScoped<ISearchWordService, SearchWordService>();

            services.AddHttpClient();

            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(ApplicationLayer).Assembly));
        }

        public static void AddBackgroundServies(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHostedService<SyncBackgroundService>();
        }

        public static void AddLogger(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, cl) =>
            {
                cl.ReadFrom.Configuration(ctx.Configuration);

                cl.Enrich.FromLogContext();
                cl.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);
                cl.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName);

                cl.WriteTo.MongoDBBson(cfg =>
                {
                    var identity = new MongoInternalIdentity(ctx.Configuration["SerilogMongo:AuthDatabase"], ctx.Configuration["SerilogMongo:UserName"]);
                    var evidence = new PasswordEvidence(ctx.Configuration["SerilogMongo:Password"]);

                    var mongoDbSettings = new MongoClientSettings
                    {
                        UseTls = false,
                        Credential = new MongoCredential(ctx.Configuration["SerilogMongo:AuthMechanisam"], identity, evidence),
                        Server = new MongoServerAddress(ctx.Configuration["SerilogMongo:Host"], ctx.Configuration.GetValue<int>("SerilogMongo:Port")),
                    };

                    var mongoDbInstance = new MongoClient(mongoDbSettings).GetDatabase(ctx.Configuration["SerilogMongo:Database"]);

                    cfg.SetMongoDatabase(mongoDbInstance);
                });
            });

            Serilog.Debugging.SelfLog.Enable(msg => {
                Debug.WriteLine(msg);
            });
        }
    }
}
