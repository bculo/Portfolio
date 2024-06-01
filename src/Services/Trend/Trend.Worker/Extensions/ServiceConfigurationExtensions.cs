using MassTransit;
using Trend.Application;
using Trend.Application.Configurations.Options;

namespace Trend.Worker.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SyncBackgroundServiceOptions>(configuration.GetSection("SyncBackgroundServiceOptions"));
            
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.ConfigureHangfire(configuration, services, true);
            ApplicationLayer.ConfigureCache(configuration, services);
            ApplicationLayer.AddOpenTelemetry(configuration, services, "Trend.Worker");

            AddMessageQueue(services, configuration);
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
    }
}
