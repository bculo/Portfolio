using Keycloak.Common;
using System.Reflection;
using Trend.Application;
using Trend.Domain.Interfaces;
using Trend.Grpc.Interceptors;
using Trend.Grpc.Services;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Trend.Grpc.Extensions
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

            ConfigureAuthentication(services, configuration);

            ApplicationLayer.AddLogger(builder.Host);
            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
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
    }
}
