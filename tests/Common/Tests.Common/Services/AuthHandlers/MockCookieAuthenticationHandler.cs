using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tests.Common.Interfaces;
using Tests.Common.Interfaces.Claims;

namespace Tests.Common.Services.AuthHandlers;

public class MockCookieAuthenticationHandler : MockBaseAuthenticationHandler
{
    public MockCookieAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock, 
        IMockClaimSeeder seeder) 
            : base(options, logger, encoder, clock, seeder)
    {
    }

    protected override string SchemeName => CookieAuthenticationDefaults.AuthenticationScheme;
}