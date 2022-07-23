using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Keycloak.Common.UnitTests.Helpers;
using Keycloak.Common.Constants;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakClaimsTransformerTests
    {
        private const string APP_NAME = "Test.API";

        [Fact]
        public void TransformAsync_Should_Throw_NullReferenceException_When_Passed_Value_Null()
        {
            var instance = CreateInstance();

            Assert.ThrowsAsync<NullReferenceException>(() => instance.TransformAsync(null));
        }

        [Fact]
        public async Task TransformAsync_Should_Not_Create_Role_Claim_When_Principal_Without_Roles_Passed()
        {
            var instance = CreateInstance();

            var principal = KeycloakClaimsPrincipalUtils.CreateClaimsPrincipalWithoutRoleForUser("dorix", "morix", "dorix");

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.DoesNotContain(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
        }

        [Fact]
        public async Task TransformAsync_Should_Create_Role_Claim_When_Principal_With_Single_Realm_Role_Passed()
        {
            var instance = CreateInstance();

            string userRole = "Admin";

            var principal = KeycloakClaimsPrincipalUtils.CreateClaimsPrincipalWithRealmRoleForUser("dorix", "morix", "dorix", userRole);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == userRole);
        }

        [Fact]
        public async Task TransformAsync_Should_Create_Multiple_Role_Claims_When_Principal_With_Multiple_Realm_Roles_Passed()
        {
            var instance = CreateInstance();

            var firstUserRole = "Admin";
            var secondUserRole = "PowerAdmin";

            var principal = KeycloakClaimsPrincipalUtils.CreateClaimsPrincipalWithRealmMultipleRoleForUser("dorix", "morix", "dorix", firstUserRole, secondUserRole);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == firstUserRole);
            Assert.Contains(result.Claims, i => i.Value == secondUserRole);
        }

        private KeycloakClaimsTransformer CreateInstance()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakClaimOptions
            {
                ApplicationName = APP_NAME,
            });

            var logger = Mock.Of<ILogger<KeycloakClaimsTransformer>>();

            return new KeycloakClaimsTransformer(options, logger);
        }
    }
}