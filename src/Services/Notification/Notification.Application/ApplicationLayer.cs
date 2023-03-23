using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Constants;
using Notification.Application.Consumers.Crypto;

namespace Notification.Application
{
    public static class ApplicationLayer
    {
        public static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                ConfigureConsumers(x);

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private static void ConfigureConsumers(IBusRegistrationConfigurator config)
        {
            config.AddConsumer<CryptoPriceUpdatedConsumer>()
                .Endpoint(config =>
                {
                    config.Name = $"{MessageQueue.QueuePrefix}-{nameof(CryptoPriceUpdatedConsumer)}";
                });
        }
    }
}
