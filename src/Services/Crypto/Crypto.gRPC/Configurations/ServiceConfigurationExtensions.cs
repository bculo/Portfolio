using Crypto.Application;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.Application.Options;
using Crypto.Infrastracture;
using MassTransit;

namespace Crypto.gRPC.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfiguregRPCApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddGrpcReflection();

            ApplicationLayer.AddServices(services, configuration);

            InfrastractureLayer.AddCommonServices(services, configuration);
            InfrastractureLayer.AddPersistenceStorage(services, configuration);
            InfrastractureLayer.AddCacheMemory(services, configuration);
            InfrastractureLayer.AddClients(services, configuration);

            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));
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
