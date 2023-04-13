using Crypto.API.Filters;
using Crypto.Application;
using Crypto.Application.Options;
using Crypto.Infrastracture;
using Crypto.Infrastracture.Consumers;
using Crypto.Infrastracture.Consumers.State;
using Crypto.Infrastracture.Persistence;
using MassTransit;
using Microsoft.Data.SqlClient;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Crypto.API.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
            });

            ApplicationLayer.AddServices(services, configuration);

            InfrastractureLayer.AddCommonServices(services, configuration);
            InfrastractureLayer.AddPersistenceStorage(services, configuration);
            InfrastractureLayer.AddCacheMemory(services, configuration);
            InfrastractureLayer.AddClients(services, configuration);

            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<CryptoDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                x.AddDelayedMessageScheduler();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Crypto", false));

                x.AddSagaStateMachine<AddCryptoItemStateMachine, AddCryptoItemState, AddCryptoItemStateMachineDefinition>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<CryptoDbContext>();
                        r.UseSqlServer();
                    });

                x.AddConsumer<AddCryptoItemConsumer>();
                x.AddConsumer<CryptoVisitedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.UseDelayedMessageScheduler();
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });

            services.ConfigureVersioning(configuration);

            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .AddSource("MassTransit")
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("Crypto.API")
                            .AddTelemetrySdk()
                            .AddEnvironmentVariableDetector())
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation(opt =>
                        {
                            opt.RecordException = true;
                            opt.EnableConnectionLevelAttributes = true;
                            opt.SetDbStatementForText = true;

                            opt.Filter = cmd =>
                            {
                                if (cmd is SqlCommand command
                                    && (command.CommandText.Contains("OutboxState")
                                        || command.CommandText.Contains("InboxState")))
                                {
                                    return false;
                                }
                                return true;
                            };
                        })
                        .AddJaegerExporter(o =>
                        {
                            o.AgentHost = "localhost";
                            o.AgentPort = 6831;
                            o.MaxPayloadSizeInBytes = 4096;
                            o.ExportProcessorType = ExportProcessorType.Batch;
                            o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                            {
                                MaxQueueSize = 2048,
                                ScheduledDelayMilliseconds = 5000,
                                ExporterTimeoutMilliseconds = 30000,
                                MaxExportBatchSize = 512,
                            };
                        });
                });
        }
    }
}
