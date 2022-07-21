using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Constants
{
    internal static class KeycloakTokenConstants
    {
        public const string EXPIRE = "exp";
        public const string ISSUER = "iss";
        public const string TYPE = "typ";
        public const string FULLNAME = "name";
        public const string USERNAME = "preferred_username";
        public const string EMAIL = "email";
        public const string USERID = ClaimTypes.NameIdentifier;
        public const string ROLE = ClaimTypes.Role;
        public const string CLIENTID = "clientId";
        public const string CLIENT_HOST = "clientId";
        public const string CLIENT_ADDRESS = "clientAddress";
    }
}
