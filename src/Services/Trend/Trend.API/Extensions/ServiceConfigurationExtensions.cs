using Cache.Common;
using FluentValidation.AspNetCore;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Globalization;
using Trend.API.Filters;
using Trend.API.Policies;
using Trend.API.Services;
using Trend.Application;
using Trend.Domain.Interfaces;
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
                    .Tag("News"));
                
                opt.AddPolicy("SearchWordPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromHours(2))
                    .Tag("SearchWord"));
                
                opt.AddPolicy("SyncPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromHours(2))
                    .Tag("Sync"));
                
                opt.AddPolicy("SyncPostPolicy", policy => policy.AddPolicy<AuthPostRequestPolicy>()
                    .Expire(TimeSpan.FromMinutes(30))
                    .VaryByValue(context =>
                    {
                        context.Request.EnableBuffering();
                        
                        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                        var body = reader.ReadToEndAsync();
                        
                        context.Request.Body.Position = 0;
                        var keyVal = new KeyValuePair<string, string>("Body", body.Result);
                        return keyVal;
                    })
                    .Tag("Sync"));
            });
            builder.Services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });

            AddMessageQueue(services, configuration);
            ConfigureLocalization(services);
            ConfigureAuthentication(services, configuration);
            AddOpenTelemetry(services);

            services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            ApplicationLayer.AddLogger(builder.Host);
            CacheConfiguration.AddRedis(services, configuration);
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
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

        private static void AddOpenTelemetry(IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .AddSource("MassTransit")
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("Trend.API"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddMongoDBInstrumentation()
                        .AddJaegerExporter();
                });
        }
    }
}
