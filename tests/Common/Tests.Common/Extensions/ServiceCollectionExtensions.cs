using Keycloak.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Interfaces.Claims;
using Tests.Common.Services.AuthHandlers;
using Tests.Common.Services.Claims;

namespace Tests.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultFakeAuth(this IServiceCollection services)
    {
        services.AddSingleton<IMockClaimSeeder, MockClaimSeeder>();
        services.AddSingleton<IAuthenticationSchemeProvider, MockJwtSchemeProvider>();

        return services;
    }
}