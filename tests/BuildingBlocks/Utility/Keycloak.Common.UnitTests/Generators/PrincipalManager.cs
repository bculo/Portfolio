using AutoFixture;
using Keycloak.Common.Services;
using Keycloak.Common.UnitTests.Models;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests.Generators
{
    internal class PrincipalManager
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly KeycloakClaimsTransformer? _transformer;

        public PrincipalManager()
        {

        }

        public PrincipalManager(KeycloakClaimsTransformer transformer)
        {
            _transformer = transformer;
        }

        public ClaimsPrincipal CreateEmptyPrincipal()
        {
            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(i => i.IsAuthenticated).Returns(false);
            identityMock.Setup(i => i.Claims).Returns(Enumerable.Empty<Claim>());
            return new ClaimsPrincipal(identityMock.Object);
        }

        public async Task<ClaimsPrincipal> CreatePrincipalForClient(string clientName = "CLIENT.IDENTIFIER", string[]? clientRealmRole = null)
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
                        if(clientRealmRole != null)
                            claims.Add(new Claim("realm_access", AddRealmRoles(clientRealmRole)));
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            var newPrincipal = new ClaimsPrincipal(identity);

            if(_transformer is null)
            {
                return newPrincipal;
            }

            return await _transformer.TransformAsync(newPrincipal);
        }

        public async Task<ClaimsPrincipal> CreatePrincipalForUser(string firstName = "firstName", 
            string? lastName = "lastName", 
            string? username = "username",
            string applicationName = "APP",
            string[]? realmRoles = null, 
            string[]? appRoles = null)
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
                        if(realmRoles != null) 
                            claims.Add(new Claim("realm_access", AddRealmRoles(realmRoles)));
                        break;
                    case "resource_access":
                        if (appRoles != null)
                            claims.Add(new Claim("resource_access", AddApplicationRoles(applicationName, appRoles)));
                        break;
                    default:
                        claims.Add(new Claim(pair.Key, pair.Value!.ToString()));
                        break;
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            var newPrincipal = new ClaimsPrincipal(identity);

            if (_transformer is null)
            {
                return newPrincipal;
            }

            return await _transformer.TransformAsync(newPrincipal);
        }

        private JObject CreateUserJsonObject()
        {
            var jsonAccessToken = _fixture.Create<UserAccessToken>();
            jsonAccessToken.sub = Guid.NewGuid().ToString();
            var jsonObject = JObject.Parse(JsonConvert.SerializeObject(jsonAccessToken));
            return jsonObject;
        }

        private JObject CreateClientJsonObject()
        {
            var jsonAccessToken = _fixture.Create<ClientAccessToken>();
            jsonAccessToken.sub = Guid.NewGuid().ToString();
            var jsonObject = JObject.Parse(JsonConvert.SerializeObject(jsonAccessToken));
            return jsonObject;
        }

        private string AddRealmRoles(params string[] roles)
        {
            var realmRoles = new { roles = roles.Distinct() };
            return JsonConvert.SerializeObject(realmRoles);
        }

        private string AddApplicationRoles(string applicationName, params string[] roles)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add(applicationName, new { roles = roles });
            return JsonConvert.SerializeObject(dictionary);
        }
    }
}
