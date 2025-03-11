using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Jwt;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Tests.Common;

public static class ServiceProviderExtensions
{
    public static void AddTestAuthentication(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, JwtRsaTokenGenerator>();

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var config = scopedServices.GetService<IConfiguration>();

        var authenticationSchemeName = $"TestAuth{Guid.NewGuid()}";
        
        services.ConfigureDefaultAuthentication(new AuthOptions
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config.GetValue<string?>("AuthOptions:ValidIssuer") ?? throw new Exception("Valid issuer not found") ,
            ValidateLifetime = true,
            PublicRsaKey = config.GetValue<string?>("AuthOptions:PublicRsaKey") ?? throw new Exception("Public RSA key not found")
        }, authenticationSchemeName);
        
        services.ConfigureDefaultAuthorization(authenticationSchemeName);
    }
}