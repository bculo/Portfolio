using Carter;
using Keycloak.Common;
using Mail.Application;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Mail.API.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureMinimalApiProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.ConfigureSwagger($"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth");
        
        ApplicationLayer.AddServices(services, configuration);
        
        AddAuthentication(services, configuration);
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
}