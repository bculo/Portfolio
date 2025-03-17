using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Jwt;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Crypto.IntegrationTests;

public static class TestConfiguration
{
    public static void AddTestAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITokenGenerator, JwtRsaTokenGenerator>();
        var authenticationSchemeName = $"TestAuth{Guid.NewGuid()}";
        
        services.ConfigureDefaultAuthentication(new AuthOptions
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration.GetValue<string?>("AuthOptions:ValidIssuer") ?? throw new Exception("Valid issuer not found") ,
            ValidateLifetime = true,
            PublicRsaKey = configuration.GetValue<string?>("AuthOptions:PublicRsaKey") ?? throw new Exception("Public RSA key not found")
        }, authenticationSchemeName);
        
        services.ConfigureDefaultAuthorization(authenticationSchemeName);
    }
}