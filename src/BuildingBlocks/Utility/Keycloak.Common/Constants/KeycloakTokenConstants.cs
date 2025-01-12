using System.Security.Claims;

namespace Keycloak.Common.Constants
{
    internal static class KeycloakTokenConstants
    {
        public const string Expire = "exp";
        public const string Issuer = "iss";
        public const string Type = "typ";
        public const string Fullname = "name";
        public const string Username = "preferred_username";
        public const string Email = "email";
        public const string Userid = ClaimTypes.NameIdentifier;
        public const string Useridbackup = "sub";
        public const string Role = ClaimTypes.Role;
        public const string Clientid = "clientId";
        public const string ClientHost = "clientId";
        public const string ClientAddress = "clientAddress";
    }
}
