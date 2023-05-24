using Crypto.API.Filters;
using Crypto.Application;
using Crypto.Application.Options;
using Crypto.Infrastracture;
using Crypto.Infrastracture.Consumers;
using Crypto.Infrastracture.Consumers.State;
using Crypto.Infrastracture.Persistence;
using Keycloak.Common;
using MassTransit;
using Microsoft.Data.SqlClient;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Crypto.API.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
            });

            services.AddCors();

            ApplicationLayer.AddServices(services, configuration);

            InfrastractureLayer.AddCommonServices(services, configuration);
            InfrastractureLayer.AddPersistenceStorage(services, configuration);
            InfrastractureLayer.AddCacheMemory(services, configuration);
            InfrastractureLayer.AddClients(services, configuration);

            services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            AddAuthentication(services, configuration);
            AddMessageQueue(services, configuration);
            AddOpenTelemetry(services, configuration);
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
            services.UseKeycloakCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

            var authOptions = new AuthOptions();
            configuration.GetSection("AuthOptions").Bind(authOptions);

            services.ConfigureDefaultAuthentication(authOptions);
            services.ConfigureDefaultAuthorization();
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));
            services.Configure<SagaTimeoutOptions>(configuration.GetSection("SagaTimeoutOptions"));

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<CryptoDbContext>(o =>
                {
                    o.UseSqlServer();
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
        }

        private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .AddSource("MassTransit")
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService("Crypto.API"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation(opt =>
                        {
                            opt.RecordException = true;
                            opt.EnableConnectionLevelAttributes = true;
                            opt.SetDbStatementForText = true;

                            opt.Filter = cmd =>
                            {
                                return cmd is not SqlCommand command 
                                       || (!command.CommandText.Contains("OutboxState") 
                                           && !command.CommandText.Contains("InboxState"));
                            };
                        })
                        .AddJaegerExporter();
                });
        }
    }
}
