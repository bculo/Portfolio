using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests.Helpers
{
    internal static class PrincipalUtils
    {
        private const string KEYCLOAK_USER_RESPONSE_PATH = "Static/keycloak-user-response.json";
        private const string KEYCLOAK_CLIENT_RESPONSE_PATH = "Static/keycloak-client-response.json";

        public static ClaimsPrincipal? CreateEmptyPrincipal()
        {
            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(i => i.IsAuthenticated).Returns(false);
            identityMock.Setup(i => i.Claims).Returns(Enumerable.Empty<Claim>());

            return new ClaimsPrincipal(identityMock.Object);
        }

        public static ClaimsPrincipal CreatePrincipalForClientWithoutRoles(string clientName)
        {
            var jsonObject = CreateClientJsonObject();

            var claims = new List<Claim>();

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "clientId":
                        claims.Add(new Claim("clientId", clientName));
                        break;
                    case "realm_access":
                        break;
                    case "resource_access":
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public static ClaimsPrincipal CreatePrincipalForClientWithRole(string clientName, string clientRealmRole)
        {
            var jsonObject = CreateClientJsonObject();

            var claims = new List<Claim>();

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "clientId":
                        claims.Add(new Claim("clientId", clientName));
                        break;
                    case "realm_access":
                        claims.Add(new Claim("realm_access", AddRealmRoles(clientRealmRole)));
                        break;
                    case "resource_access":
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public static ClaimsPrincipal CreatePrincipalWithoutRoleForUser(string firstName, string lastName, string username)
        {
            var jsonObject = CreateUserJsonObject();

            var claims = new List<Claim>();

            foreach(var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "name":
                        claims.Add(new Claim("name", $"{firstName} {lastName}"));
                        break;
                    case "preferred_username":
                        claims.Add(new Claim("preferred_username", username));
                        break;
                    case "given_name":
                        claims.Add(new Claim("given_name", firstName));
                        break;
                    case "family_name":
                        claims.Add(new Claim("family_name", lastName));
                        break;
                    case "email":
                        claims.Add(new Claim("email", $"{firstName}.{lastName}@mail.com"));
                        break;
                    case "realm_access":
                        break;
                    case "resource_access":
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public static ClaimsPrincipal CreatePrincipalWithRealmRoleForUser(string firstName, string lastName, string username, string realmRole)
        {
            var jsonObject = CreateUserJsonObject();

            var claims = new List<Claim>();

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "name":
                        claims.Add(new Claim("name", $"{firstName} {lastName}"));
                        break;
                    case "preferred_username":
                        claims.Add(new Claim("preferred_username", username));
                        break;
                    case "given_name":
                        claims.Add(new Claim("given_name", firstName));
                        break;
                    case "family_name":
                        claims.Add(new Claim("family_name", lastName));
                        break;
                    case "email":
                        claims.Add(new Claim("email", $"{firstName}.{lastName}@mail.com"));
                        break;
                    case "realm_access":
                        claims.Add(new Claim("realm_access", AddRealmRoles(realmRole)));
                        break;
                    case "resource_access":
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public static ClaimsPrincipal CreatePrincipalWithRealmMultipleRolesForUser(string firstName, string lastName, string username, params string[] realmRoles)
        {
            var jsonObject = CreateUserJsonObject();

            var claims = new List<Claim>();

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "name":
                        claims.Add(new Claim("name", $"{firstName} {lastName}"));
                        break;
                    case "preferred_username":
                        claims.Add(new Claim("preferred_username", username));
                        break;
                    case "given_name":
                        claims.Add(new Claim("given_name", firstName));
                        break;
                    case "family_name":
                        claims.Add(new Claim("family_name", lastName));
                        break;
                    case "email":
                        claims.Add(new Claim("email", $"{firstName}.{lastName}@mail.com"));
                        break;
                    case "realm_access":
                        claims.Add(new Claim("realm_access", AddRealmRoles(realmRoles)));
                        break;
                    case "resource_access":
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public static ClaimsPrincipal CreatePrincipalWithApplicationRoleForUser(string firstName, string lastName, string username, string appName, string appRole)
        {
            var jsonObject = CreateUserJsonObject();

            var claims = new List<Claim>();

            foreach (var pair in jsonObject)
            {
                switch (pair.Key)
                {
                    case "name":
                        claims.Add(new Claim("name", $"{firstName} {lastName}"));
                        break;
                    case "preferred_username":
                        claims.Add(new Claim("preferred_username", username));
                        break;
                    case "given_name":
                        claims.Add(new Claim("given_name", firstName));
                        break;
                    case "family_name":
                        claims.Add(new Claim("family_name", lastName));
                        break;
                    case "email":
                        claims.Add(new Claim("email", $"{firstName}.{lastName}@mail.com"));
                        break;
                    case "realm_access":
                        break;
                    case "resource_access":
                        claims.Add(new Claim("resource_access", AddApplicationRoles(appName, appRole)));
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        private static JObject CreateUserJsonObject()
        {
            if (!File.Exists(KEYCLOAK_USER_RESPONSE_PATH))
            {
                throw new FileNotFoundException();
            }

            var jsonUserResponse = File.ReadAllText(KEYCLOAK_USER_RESPONSE_PATH);

            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonUserResponse);

            if (jsonObject is null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            return jsonObject;
        }

        private static JObject CreateClientJsonObject()
        {
            if (!File.Exists(KEYCLOAK_CLIENT_RESPONSE_PATH))
            {
                throw new FileNotFoundException();
            }

            var jsonUserResponse = File.ReadAllText(KEYCLOAK_CLIENT_RESPONSE_PATH);

            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonUserResponse);

            if (jsonObject is null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            return jsonObject;
        }

        private static string AddRealmRoles(params string[] roles)
        {
            var realmRoles = new
            {
                roles = roles.Distinct()
            };

            return JsonConvert.SerializeObject(realmRoles);
        }

        private static string AddApplicationRoles(string applicationName, params string[] roles)
        {
            var dictionary = new Dictionary<string, object>();

            dictionary.Add(applicationName, new { roles = roles });

            return JsonConvert.SerializeObject(dictionary);
        }

    }
}
