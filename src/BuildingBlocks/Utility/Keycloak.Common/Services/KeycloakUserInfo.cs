using Auth0.Abstract.Contracts;
using Keycloak.Common.Constants;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Keycloak.Common.Services
{
    internal sealed class KeycloakUserInfo(IHttpContextAccessor accessor) : IAuth0AccessTokenReader
    {
        public ClaimsPrincipal Claims => accessor.HttpContext?.User;

        public bool IsAuthenticated()
        {
            return Claims?.Identity?.IsAuthenticated ?? false;
        }

        public Guid GetIdentifier()
        {
            var guidAsStringMain = Claims?.FindFirst(KeycloakTokenConstants.Userid)?.Value;
            var guidAsStringBackup = Claims?.FindFirst(KeycloakTokenConstants.Useridbackup)?.Value;

            var guidAsString = guidAsStringMain ?? guidAsStringBackup ?? null;

            if (string.IsNullOrEmpty(guidAsString))
            {
                return Guid.Empty;
            }

            if(Guid.TryParse(guidAsString, out var userId))
            {
                return userId;
            }

            return Guid.Empty;
        }

        public string GetFullName()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.Fullname)?.Value;
        }

        public string GetEmail()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.Email)?.Value;
        }

        public string GetUserName()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.Username)?.Value;
        }

        public string GetIssuer()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.Issuer)?.Value;
        }

        public IEnumerable<string> GetRoles()
        {
            return Claims?.FindAll(KeycloakTokenConstants.Role)?.Select(i => i.Value)?.ToList() ?? Enumerable.Empty<string>();
        }

        public bool IsInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            return GetRoles().Any(i => i == roleName);
        }

        public bool IsApplication()
        {
            return GetRoles().Any(i => i == "Application");
        }

        public string GetClientId()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.Clientid)?.Value;
        }

        public IPAddress GetClientAddress()
        {
            var ipString = Claims?.FindFirst(KeycloakTokenConstants.ClientAddress)?.Value;

            if (string.IsNullOrEmpty(ipString))
            {
                return null;
            }

            if(IPAddress.TryParse(ipString, out IPAddress ipAddress))
            {
                return ipAddress;
            }

            return null;
        }
    }
}
