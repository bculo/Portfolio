using Crypto.Application;
using Crypto.Application.Common.Options;
using Crypto.Infrastructure;
using Crypto.Infrastructure.Consumers;
using Crypto.Infrastructure.Consumers.State;
using Crypto.Infrastructure.Persistence;
using Keycloak.Common;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Crypto.API.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            
            services.AddControllers();

            services.AddCors();
            services.AddProblemDetails();

            ApplicationLayer.AddServices(services, configuration);
            InfrastructureLayer.AddServices(services, configuration);

            services.ConfigureSwagger(
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration["KeycloakOptions:ApplicationName"]);

            AddAuthentication(services, configuration);
            AddMessageQueue(services, configuration);
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
            services.UseKeycloakClientCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

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
                x.AddDelayedMessageScheduler();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: configuration["QueueOptions:Prefix"], false)); 
                
                x.AddSagaStateMachine<AddCryptoItemStateMachine, AddCryptoItemState>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<CryptoDbContext>();
                        r.LockStatementProvider = new PostgresLockStatementProvider();
                        r.UsePostgres();
                    })
                    .Endpoint(config =>
                    {
                        config.Temporary = configuration.GetValue<bool>("QueueOptions:Temporary");
                    });

                x.AddConsumer<AddCryptoItemConsumer>()
                    .Endpoint(config =>
                    {
                        config.Temporary = configuration.GetValue<bool>("QueueOptions:Temporary");
                    });
                
                x.AddConsumer<CryptoVisitedConsumer>()
                    .Endpoint(config =>
                    {
                        config.Temporary = configuration.GetValue<bool>("QueueOptions:Temporary");
                    });
                
                x.AddConsumer<EvictRedisListRequestConsumer>()
                    .Endpoint(config =>
                    {
                        config.Temporary = configuration.GetValue<bool>("QueueOptions:Temporary");
                    });
                x.AddConsumer<PriceUpdatedConsumer>()
                    .Endpoint(config =>
                    {
                        config.Temporary = configuration.GetValue<bool>("QueueOptions:Temporary");
                    });

                x.UsingRabbitMq((context, config) =>
                {
                    config.UseDelayedMessageScheduler();
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
