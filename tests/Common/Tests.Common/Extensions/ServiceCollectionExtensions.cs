using Keycloak.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Interfaces.Claims;
using Tests.Common.Services.AuthHandlers;
using Tests.Common.Services.Claims;

namespace Tests.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static T GetConfigValue<T>(this IServiceCollection services, string name)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var config = scopedServices.GetService<IConfiguration>();
        return config.GetValue<T>(name);
    }
}