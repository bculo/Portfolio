using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tests.Common.Interfaces;
using Tests.Common.Interfaces.Claims;

namespace Tests.Common.Services.AuthHandlers;

public abstract class MockBaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IMockClaimSeeder _seeder;
    
    protected abstract string SchemeName { get; }
    
    protected MockBaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder,
        IMockClaimSeeder seeder) 
        : base(options, logger, encoder)
    {
        _seeder = seeder;
    }
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue("UserAuthType", out var values))
        {
            return Task.FromResult(
                AuthenticateResult.Fail(new AuthenticationException("UserAuthType not provided in header")));
        }

        var value = values[0];

        if (value is null || !int.TryParse(value, out var userTypeIdentifier))
        {
            return Task.FromResult(
                AuthenticateResult.Fail(new AuthenticationException("UserAuthType with invalid value provided")));
        }

        var claims = _seeder.GetClaims(userTypeIdentifier);
        var claimIdentity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(claimIdentity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}