using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Cache.Redis.Common;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using Trend.Application;
using Trend.Application.Configurations.Options;
using Trend.Application.Consumers;

namespace Trend.Worker.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SyncBackgroundServiceOptions>(configuration.GetSection("SyncBackgroundServiceOptions"));
            
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
            ApplicationLayer.ConfigureHangfire(configuration, services, true);
            ApplicationLayer.ConfigureCache(configuration, services);
            ApplicationLayer.AddOpenTelemetry(configuration, services, "Trend.Worker");

            AddMessageQueue(services, configuration);
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Trend", false));

                x.AddConsumer<SyncExecutedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
