using Keycloak.Common.Constants;
using Keycloak.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Services
{
    public class KeycloackUserInfo : IKeycloakUser
    {
        protected readonly IHttpContextAccessor _accessor;

        protected ClaimsPrincipal Claims => _accessor.HttpContext?.User;

        public KeycloackUserInfo(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public virtual bool IsAuthenticated()
        {
            return Claims?.Identity?.IsAuthenticated ?? false;
        }

        public virtual Guid? GetIdentifier()
        {
            var guidAsString = Claims?.FindFirst(KeycloackTokenConstants.USERID)?.Value;

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

        public virtual string? FullName()
        {
            return Claims?.FindFirst(KeycloackTokenConstants.FULLNAME)?.Value;
        }

        public virtual string? Email()
        {
            return Claims?.FindFirst(KeycloackTokenConstants.EMAIL)?.Value;
        }

        public virtual string? UserName()
        {
            return Claims?.FindFirst(KeycloackTokenConstants.USERNAME)?.Value;
        }

        public virtual string? GetIssuer()
        {
            return Claims?.FindFirst(KeycloackTokenConstants.ISSUER)?.Value;
        }
    }
}
