using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Constants
{
    public static class KeycloackTokenConstants
    {
        public const string EXPIRE = "exp";
        public const string ISSUER = "iss";
        public const string TYPE = "typ";
        public const string FULLNAME = "name";
        public const string USERNAME = "preferred_username";
        public const string EMAIL = "email";
        public const string USERID = ClaimTypes.NameIdentifier;
    }
}
