using System.Security.Claims;
using Keycloak.Common.Extensions;
using Keycloak.Common.Options;
using Keycloak.Common.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Keycloak.Common.Services
{
    /// <summary>
    /// Flatten resource_access because Microsoft identity model doesn't support nested claims
    /// </summary>
    internal class KeycloakClaimsTransformer(
        IOptions<KeycloakClaimOptions> options,
        IOptions<KeycloakTokenOptions> optionsToken,
        ILogger<KeycloakClaimsTransformer> logger,
        IHttpClientFactory factory,
        IHttpRequestContextService requestContext)
        : IClaimsTransformation
    {
        private readonly KeycloakClaimOptions _options = options.Value;
        private readonly KeycloakTokenOptions _tokenOptions = optionsToken.Value;

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is not ClaimsIdentity claimsIdentity)
            {
                logger.LogTrace("ClaimsIdentity instance is null");
                return principal;
            }

            if (requestContext.HasJwtToken)
            {
                logger.LogTrace("Calling UserInfo endpoint");
                await GetUserInfo(claimsIdentity);
            }
            
            return principal;
        }

        private async Task GetUserInfo(ClaimsIdentity claimsIdentity)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", requestContext.Jwt);
            var userinfoPath = Path.Join(_tokenOptions.AuthorizationServerUrl, "/protocol/openid-connect/userinfo");
            var response = await client.GetAsync(userinfoPath);
            response.EnsureSuccessStatusCode();
            var parsedResponse = await response.HandleResponse<CustomUserInfo>();
            HandleRealmRoles(parsedResponse, claimsIdentity);
        }
        
        private void HandleRealmRoles(CustomUserInfo info, ClaimsIdentity claimsIdentity)
        {
            foreach (var role in info.RealmRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
    }
}
