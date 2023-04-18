using Crypto.Application;
using Crypto.Application.Options;
using Crypto.Infrastracture;
using Crypto.Infrastracture.Consumers;
using Crypto.Infrastracture.Persistence;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.BackgroundUpdate.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationLayer.AddServices(services, configuration);

            InfrastractureLayer.AddCommonServices(services, configuration);
            InfrastractureLayer.AddPersistenceStorage(services, configuration);
            InfrastractureLayer.AddCacheMemory(services, configuration);
            InfrastractureLayer.AddClients(services, configuration);

            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));
            services.Configure<CryptoUpdateOptions>(configuration.GetSection("CryptoUpdateOptions"));

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<CryptoDbContext>(o =>
                {
                    o.UseSqlServer();
                });

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Crypto", false));

                x.AddConsumer<UpdateCryptoItemsPriceConsumer, UpdateCryptoItemsPriceConsumerDefinition>();
                x.AddConsumer<CryptoPriceUpdatedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
