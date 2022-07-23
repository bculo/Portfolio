using Keycloak.Common.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Services
{
    /// <summary>
    /// Flatten resource_access because Microsoft identity model doesn't support nested claims
    /// </summary>
    internal class KeycloakClaimsTransformer : IClaimsTransformation
    {
        private readonly KeycloakClaimOptions _options;
        private readonly ILogger<KeycloakClaimsTransformer> _logger;

        public KeycloakClaimsTransformer(IOptions<KeycloakClaimOptions> options, ILogger<KeycloakClaimsTransformer> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            _logger.LogTrace("Method {0} in service {1} called", nameof(TransformAsync), nameof(KeycloakClaimsTransformer));

            ClaimsIdentity? claimsIdentity = principal.Identity as ClaimsIdentity;

            if(claimsIdentity == null) 
            {
                _logger.LogWarning("ClaimsIdentity instance is null");

                return Task.FromResult(principal);
            }

            HandleApplicationRoles(claimsIdentity);

            HandleRealmRoles(claimsIdentity);

            return Task.FromResult(principal);
        }

        private void HandleApplicationRoles(ClaimsIdentity claimsIdentity)
        {
            _logger.LogTrace("Method {0} in service {1} called", nameof(HandleApplicationRoles), nameof(KeycloakClaimsTransformer));

            if (claimsIdentity.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
            {
                var userAppRoles = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");

                var content = Newtonsoft.Json.Linq.JObject.Parse(userAppRoles!.Value);

                if (content.ContainsKey(_options.ApplicationName!))
                {
                    foreach (var role in content[_options.ApplicationName]!["roles"]!)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                    }
                }
            }
        }

        private void HandleRealmRoles(ClaimsIdentity claimsIdentity)
        {
            _logger.LogTrace("Method {0} in service {1} called", nameof(HandleRealmRoles), nameof(KeycloakClaimsTransformer));

            if (claimsIdentity.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "realm_access"))
            {
                var userRealmRoles = claimsIdentity.FindFirst((claim) => claim.Type == "realm_access");

                var content = Newtonsoft.Json.Linq.JObject.Parse(userRealmRoles!.Value);

                if (content.ContainsKey("roles"))
                {
                    foreach (var role in content["roles"]!)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                    }
                }
            }
        }
    }
}
