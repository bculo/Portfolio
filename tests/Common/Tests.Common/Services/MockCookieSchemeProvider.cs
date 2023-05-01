using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Tests.Common.Services;

public class MockCookieSchemeProvider : AuthenticationSchemeProvider
{
    public MockCookieSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
    {
    }

    protected MockCookieSchemeProvider(
        IOptions<AuthenticationOptions> options, 
        IDictionary<string, AuthenticationScheme> schemes) 
        : base(options, schemes)
    {
    }
        
    public override Task<AuthenticationScheme?> GetSchemeAsync(string name)
    {
        AuthenticationScheme scheme = new(
            JwtBearerDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme,
            typeof(MockJwtAuthenticationHandler)
        );

        return Task.FromResult(scheme ?? null);
    }
}