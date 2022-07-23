using Auth0.Abstract.Contracts;
using Keycloak.Common.Constants;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Keycloak.Common.Services
{
    internal class KeycloakUserInfo : IAuth0AccessTokenReader
    {
        protected readonly IHttpContextAccessor _accessor;

        protected ClaimsPrincipal Claims => _accessor.HttpContext?.User;

        public KeycloakUserInfo(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public virtual bool IsAuthenticated()
        {
            return Claims?.Identity?.IsAuthenticated ?? false;
        }

        public virtual Guid? GetIdentifier()
        {
            var guidAsString = Claims?.FindFirst(KeycloakTokenConstants.USERID)?.Value;

            if (string.IsNullOrEmpty(guidAsString))
            {
                return null;
            }

            if(Guid.TryParse(guidAsString, out var userId))
            {
                return userId;
            }

            return null;
        }

        public virtual string? GetFullName()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.FULLNAME)?.Value;
        }

        public virtual string? GetEmail()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.EMAIL)?.Value;
        }

        public virtual string? GetUserName()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.USERNAME)?.Value;
        }

        public virtual string? GetIssuer()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.ISSUER)?.Value;
        }

        public IEnumerable<string> GetRoles()
        {
            return Claims?.FindAll(KeycloakTokenConstants.ROLE)?.Select(i => i.Value)?.ToList() ?? Enumerable.Empty<string>();
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

        public string? GetClientId()
        {
            return Claims?.FindFirst(KeycloakTokenConstants.CLIENTID)?.Value;
        }

        public IPAddress? GetClientAddress()
        {
            var ipString = Claims?.FindFirst(KeycloakTokenConstants.CLIENT_ADDRESS)?.Value;

            if (string.IsNullOrEmpty(ipString))
            {
                return null;
            }

            if(IPAddress.TryParse(ipString, out IPAddress? ipAddress))
            {
                return ipAddress;
            }

            return null;
        }
    }
}
