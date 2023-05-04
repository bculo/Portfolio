﻿using System.Diagnostics;
using FluentValidation.AspNetCore;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Trend.API.Filters;
using Trend.API.Filters.Action;
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
            })
            .AddFluentValidation();

            services.AddCors();
            services.AddScoped<CacheActionFilter>();

            AddMessageQueue(services, configuration);
            ConfigureLocalization(services, configuration);
            ConfigureAuthentication(services, configuration);
            AddOpenTelemetry(services, configuration);

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
                            .AddService("Trend.API"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddMongoDBInstrumentation()
                        .AddJaegerExporter();
                });
        }
    }
}
