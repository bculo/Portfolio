using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Stock.Application;
using Stock.Application.Infrastructure.Consumers;
using Stock.Application.Interfaces;
using Stock.Worker.Jobs;
using Stock.Worker.Services;

namespace Stock.Worker.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Configure all required services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStockUser, WorkerUserService>();

            ApplicationLayer.AddServices(services, configuration);
            ApplicationLayer.AddPersistence(services, configuration);
            ApplicationLayer.AddServices(services, configuration);
            ApplicationLayer.AddClients(services, configuration);

            ConfigureHangfire(services, configuration);
            ConfigureMessageQueue(services, configuration);
        }

        private static void ConfigureHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UsePostgreSqlStorage(configuration.GetConnectionString("StockDatabase"));
            });

            services.AddHangfireServer();

            services.AddScoped<IPriceUpdateJobService, UpdateStockPriceHangfireJob>();
        }

        private static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Stock", false));

                x.AddConsumer<BatchForUpdatePreparedConsumer>(opt =>
                {
                    opt.ConcurrentMessageLimit = 5;
                });

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
