using MassTransit;
using Stock.API.Services;
using Stock.Application;
using Stock.Application.Interfaces;

namespace Stock.API.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IStockUser, CurrentUserService>();

            ApplicationLayer.AddServices(services, configuration);
            ApplicationLayer.AddClients(services, configuration);
            ApplicationLayer.AddPersistence(services, configuration);

            ConfigureMessageQueue(services, configuration);
        }

        private static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                });
            });
        }
    }
}
