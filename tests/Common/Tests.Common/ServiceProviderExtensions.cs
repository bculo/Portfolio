using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Jwt;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Tests.Common;

public static class ServiceProviderExtensions
{
    public static void ConfigureWithConfig(this IServiceCollection services,
        Action<IConfiguration> serviceConfiguration)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var config = scopedServices.GetService<IConfiguration>();
        
        serviceConfiguration.Invoke(config);
    }
}