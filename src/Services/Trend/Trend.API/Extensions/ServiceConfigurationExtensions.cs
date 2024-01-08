using Cache.Common;
using FluentValidation.AspNetCore;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Globalization;
using Microsoft.AspNetCore.Http.Features;
using OpenTelemetry.Metrics;
using StackExchange.Redis;
using Trend.API.Filters;
using Trend.API.Services;
using Trend.Application;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Utils;
using WebProject.Common.CachePolicies;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Trend.API.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApiProject(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddControllers(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
            });

            services.AddCors();
            services.AddFluentValidationAutoValidation();
            services.AddOutputCache(opt =>
            {
                opt.AddBasePolicy(policy => policy
                    .Expire(TimeSpan.FromSeconds(30)));
                
                opt.AddPolicy("NewsPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromDays(1))
                    .Tag(CacheTags.NEWS));
                
                opt.AddPolicy("DictionaryPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromDays(1))
                    .Tag(CacheTags.DICTIONARY));
                
                opt.AddPolicy("SearchWordPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromHours(2))
                    .Tag(CacheTags.SEARCH_WORD));
                
                opt.AddPolicy("SyncPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromHours(2))
                    .Tag(CacheTags.SYNC));
                
                opt.AddPolicy("SyncPostPolicy", policy => policy.AddPolicy<AuthPostRequestPolicy>()
                    .Expire(TimeSpan.FromMinutes(30))
                    .Tag(CacheTags.SYNC));
                
                
                opt.AddPolicy("WordPostPolicy", policy => policy.AddPolicy<AuthPostRequestPolicy>()
                    .Expire(TimeSpan.FromMinutes(30))
                    .Tag(CacheTags.SEARCH_WORD));
            });
            
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var multiplexer = CacheConfiguration.AddConnectionMultiplexer(services, redisConnectionString);
            builder.Services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = configuration["RedisOptions:InstanceName"];
                options.ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer);
            });

            AddMessageQueue(services, configuration);
            ConfigureLocalization(services);
            ConfigureAuthentication(services, configuration);
            AddOpenTelemetry(services, configuration, multiplexer);

            services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            ApplicationLayer.AddLogger(builder.Host);
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
            ApplicationLayer.ConfigureHangfire(configuration, services);

            StorageSeedUtils.SeedBlobStorage(services);
        }

        private static void ConfigureLocalization(IServiceCollection services)
        {
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var hrCulture = new CultureInfo("hr");
                var enCulture = new CultureInfo("en");
                var supportedCultures = new[]
                {
                    hrCulture,
                    enCulture
                };
                opts.DefaultRequestCulture = new RequestCulture(enCulture, enCulture);
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICurrentUser, UserService>();

            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
            services.UseKeycloakCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

            var authOptions = new AuthOptions();
            configuration.GetSection("AuthOptions").Bind(authOptions);

            services.ConfigureDefaultAuthentication(authOptions);
            services.ConfigureDefaultAuthorization();
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((_, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                });
            });
        }

        private static void AddOpenTelemetry(IServiceCollection services, 
            IConfiguration config,
            IConnectionMultiplexer multiplexer = null)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                {
                    resource.AddService("Trend.API");
                })
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel"))
                .WithTracing(tracing =>
                {
                    tracing.AddSource("MassTransit");
                    tracing.AddMongoDBInstrumentation();
                    tracing.AddAspNetCoreInstrumentation();
                    tracing.AddHttpClientInstrumentation();

                    if (multiplexer is not null)
                    {
                        tracing.AddRedisInstrumentation(multiplexer);
                    }

                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(config["OpenTelemetry:OtlpExporter"] 
                                               ?? throw new ArgumentNullException());
                    });
                    tracing.AddConsoleExporter();
                });
        }
    }
}
