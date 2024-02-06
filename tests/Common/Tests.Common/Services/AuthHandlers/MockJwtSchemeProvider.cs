using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Tests.Common.Services.AuthHandlers;

public class MockJwtSchemeProvider : AuthenticationSchemeProvider
{
    public MockJwtSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
    {
    }

    protected MockJwtSchemeProvider(
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