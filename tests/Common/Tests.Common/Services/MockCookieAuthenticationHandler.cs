using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tests.Common.Interfaces;

namespace Tests.Common.Services;

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

    public override string SchemeName => CookieAuthenticationDefaults.AuthenticationScheme;
}