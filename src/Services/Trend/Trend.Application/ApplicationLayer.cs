using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
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
using Trend.Domain.Interfaces;

namespace Trend.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });

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
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped(typeof(ILanguageService<>), typeof(LanguageService<>));

            services.AddHttpClient();

            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(ApplicationLayer).Assembly));
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

        public static void ConfigureAuth(IServiceCollection services, IConfiguration configuration)
        {
            //NOTE: Update System.IdentityModel.Tokens.Jwt to newest version to fix bux -> Method not found: 'Void Microsoft.IdentityModel.Tokens.InternalValidators.ValidateLifetimeAndIssuerAfterSignatureNotValidatedJwt(Microsoft.IdentityModel.Tokens.SecurityToken, System.Nullable`1<System.DateTime>, System.Nullable`1<System.DateTime>, System.String, Microsoft.IdentityModel.Tokens.TokenValidationParameters, System.Text.StringBuilder)'.

            services.UseKeycloak(configuration, "KeycloakOptions");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = configuration.GetValue<bool>("AuthOptions:ValidateAudience"),
                    ValidateIssuer = configuration.GetValue<bool>("AuthOptions:ValidateIssuer"),
                    ValidIssuers = new[] { configuration["AuthOptions:ValidIssuer"] },
                    ValidateIssuerSigningKey = configuration.GetValue<bool>("AuthOptions:ValidateIssuerSigningKey"),
                    IssuerSigningKey = RsaUtils.ImportSubjectPublicKeyInfo(configuration["AuthOptions:PublicRsaKey"]),
                    ValidateLifetime = configuration.GetValue<bool>("AuthOptions:ValidateLifetime")
                };

                //Add JWT events
                opt.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = c =>
                    {
                        Console.WriteLine("User successfully authenticated");
                        return Task.CompletedTask;
                    },
                };
            });
        }
    }
}
