using System.Globalization;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Stock.Application;
using Stock.Application.Interfaces.User;
using Stock.Infrastructure;
using Stock.Infrastructure.Consumers;
using Stock.Worker.Jobs;
using Stock.Worker.Services;

namespace Stock.Worker.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStockUser, WorkerUserService>();

            ApplicationLayer.AddServices(services, configuration);
            InfrastructureLayer.AddServices(services, configuration);

            AddHangfireServer(services, configuration);
            AddMessageQueue(services, configuration);
        }

        private static void AddHangfireServer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfireServer();
            services.AddScoped<ICreateBatchJob, CreateBatchJob>();
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "stock", false));

                x.AddConsumer<UpdateBatchPreparedConsumer>();
                x.AddConsumer<PriceUpdatedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
