using FluentValidation.AspNetCore;
using Keycloak.Common;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Trend.API.Filters;
using Trend.API.Filters.Action;
using Trend.API.Services;
using Trend.Application;
using Trend.Domain.Interfaces;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

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
            })
            .AddFluentValidation();

            services.AddCors();
            services.AddScoped<CacheActionFilter>();

            ConfigureLocalization(services, configuration);
            ConfigureAuthentication(services, configuration);

            services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            ApplicationLayer.AddLogger(builder.Host);
            ApplicationLayer.AddCache(configuration, services);
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
        }

        private static void ConfigureLocalization(IServiceCollection services, IConfiguration configuration)
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
    }
}
