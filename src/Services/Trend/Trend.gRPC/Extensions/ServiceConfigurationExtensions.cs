using Keycloak.Common;
using MassTransit;
using System.Reflection;
using Cache.Redis.Common;
using Trend.Application;
using Trend.Application.Interfaces;
using Trend.gRPC.Interceptors;
using Trend.gRPC.Services;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Trend.gRPC.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfiguregRPCProject(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            builder.Services.AddGrpc(opt =>
            {
                opt.Interceptors.Add<TrendGrpExceptionInterceptor>();
                opt.EnableDetailedErrors = true;
            });
            builder.Services.AddGrpcReflection();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            AddMessageQueue(services, configuration);
            ConfigureAuthentication(services, configuration);

            ApplicationLayer.AddLogger(builder.Host);
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
            var multiplexer = ApplicationLayer.ConfigureCache(configuration, services);
            ApplicationLayer.AddOpenTelemetry(configuration, services, "Trend.gRPC", multiplexer);
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICurrentUser, UserService>();

            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
            services.UseKeycloakCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

            var authOptions = new AuthOptions();
            configuration.GetSection("AuthOptions").Bind(authOptions);

            services.ConfigureDefaultAuthentication(authOptions);
            services.ConfigureDefaultAuthorization();
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((_, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                });
            });
        }
    }
}
