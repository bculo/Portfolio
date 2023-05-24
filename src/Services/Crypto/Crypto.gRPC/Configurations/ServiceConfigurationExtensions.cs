using Crypto.Application;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.Application.Options;
using Crypto.Infrastracture;
using MassTransit;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
            
            AddOpenTelemetry(services, configuration);
        }
        
        private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .AddSource("MassTransit")
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("Crypto.gRPC"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddJaegerExporter();
                });
        }
    }
}
