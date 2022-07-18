using Keycloak.Common.Options;
using Microsoft.AspNetCore.Authentication;
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
    public class KeycloakClaimsTransformer : IClaimsTransformation
    {
        private readonly KeycloakOptions _options;

        public KeycloakClaimsTransformer(IOptions<KeycloakOptions> options)
        {
            _options = options.Value;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity? claimsIdentity = principal.Identity as ClaimsIdentity;

            if(claimsIdentity == null) 
            {
                return Task.FromResult(principal);
            }

            if (claimsIdentity.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
            {
                var userRole = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");

                var content = Newtonsoft.Json.Linq.JObject.Parse(userRole!.Value);

                foreach (var role in content[_options!.ApplicationName]["roles"])
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            return Task.FromResult(principal);
        }
    }
}
