using Cache.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Stock.API.Filters;
using Stock.API.Services;
using Stock.Application;
using Stock.Application.Interfaces;
using Stock.Infrastructure;
using System.Globalization;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Stock.API.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
            });

            services.AddScoped<IStockUser, CurrentUserService>();

            services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            ApplicationLayer.AddServices(services, configuration);
            InfrastructureLayer.AddServices(services, configuration);

            AddMessageQueue(services, configuration);
            AddAuthentication(services, configuration);
            AddOpenTelemetry(services, configuration);

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UsePostgreSqlStorage(configuration.GetConnectionString("StockDatabase"));
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options => 
            {
                var supportedCultures = new List<CultureInfo> 
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("hr-HR")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
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
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                });
            });
        }

        private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .AddSource("MassTransit")
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("Stock.API"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddJaegerExporter();
                });
        }
    }
}
