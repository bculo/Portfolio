using Keycloak.Common;
using Tracker.API.Filters;
using Tracker.Application;
using Tracker.Infrastructure;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Tracker.API.Configurations;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });

        services.AddCors();
        services.ConfigureSwaggerAsEndpoints();
        
        ApplicationLayer.AddServices(services, configuration);
        InfrastructureLayer.AddServices(services, configuration);
        
        // AddAuthentication(services, configuration);
    }
    
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["AuthOptions:ApplicationName"]);
        //services.UseKeycloakCredentialFlowService(configuration["AuthOptions:AuthorizationServerUrl"]);

        var authOptions = new AuthOptions();
        configuration.GetSection("AuthOptions").Bind(authOptions);

        services.ConfigureDefaultAuthentication(authOptions);
        services.ConfigureDefaultAuthorization();
    }
}