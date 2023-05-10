using Carter;
using Mail.Application;

namespace Mail.API.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureMinimalApiProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        
        ApplicationLayer.AddServices(services, configuration);
    }
}