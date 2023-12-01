using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Trend.Application;
using Trend.Application.Configurations.Options;
using Trend.Application.Consumers;
using Trend.Domain.Interfaces;

namespace Trend.BackgroundSync.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SyncBackgroundServiceOptions>(configuration.GetSection("SyncBackgroundServiceOptions"));

            services.AddOutputCache();
            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });
            
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);

            AddMessageQueue(services, configuration);
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Trend", false));

                x.AddConsumer<ExecuteNewsSyncConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
