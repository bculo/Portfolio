using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tests.Common.Interfaces.Claims;

namespace Tests.Common.Services.AuthHandlers;

public class MockJwtAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IMockClaimSeeder seeder)
    : MockBaseAuthenticationHandler(options, logger, encoder, seeder)
{
    protected override string SchemeName => JwtBearerDefaults.AuthenticationScheme;
}