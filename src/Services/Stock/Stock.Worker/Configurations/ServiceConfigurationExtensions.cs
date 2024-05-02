using Hangfire;
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
            InfrastructureLayer.AddOpenTelemetry(services, configuration, "stock.worker");
            InfrastructureLayer.AddMessageQueue(services, configuration, x =>
            {
                x.AddConsumer<UpdateBatchPreparedConsumer>();
                x.AddConsumer<PriceUpdatedConsumer>();
            });
            
            AddHangfireServer(services, configuration);
        }

        private static void AddHangfireServer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfireServer();
            services.AddScoped<ICreateBatchJob, CreateBatchJob>();
        }
    }
}
