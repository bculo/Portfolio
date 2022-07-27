using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Keycloak.Common.UnitTests.Helpers;
using Keycloak.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakClaimsTransformerTests
    {
        [Fact]
        public async Task TransformAsync_ShouldThrowNullReferenceException_WhenPassedValueNull()
        {
            var instance = CreateInstance();

            await Assert.ThrowsAsync<NullReferenceException>(() => instance.TransformAsync(null));
        }

        [Fact]
        public async Task TransformAsync_ShouldNotCreateRoleClaim_WhenPrincipalWithoutRolesPassed()
        {
            var instance = CreateInstance();
            var principal = PrincipalUtils.CreatePrincipalWithoutRoleForUser("dorix", "morix", "dorix");

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.DoesNotContain(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
        }

        [Fact]
        public async Task TransformAsync_ShouldCreateRoleClaim_WhenPrincipalWithSingleRealmRolePassed()
        {
            var instance = CreateInstance();
            string userRole = "Admin";
            var principal = PrincipalUtils.CreatePrincipalWithRealmRoleForUser("dorix", "morix", "dorix", userRole);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == userRole);
        }

        [Fact]
        public async Task TransformAsync_ShouldCreateMultipleRoleClaims_WhenPrincipalWithMultipleRealmRolesPassed()
        {
            var instance = CreateInstance();
            var firstUserRole = "Admin";
            var secondUserRole = "PowerAdmin";
            var principal = PrincipalUtils.CreatePrincipalWithRealmMultipleRolesForUser("dorix", "morix", "dorix", firstUserRole, secondUserRole);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == firstUserRole);
            Assert.Contains(result.Claims, i => i.Value == secondUserRole);
        }

        [Fact]
        public async Task TransformAsync_ShouldCreateRoleClaim_WhenPrincipalWithSingleApplicationRolePassed()
        {
            var applicationName = "TEST.API";
            var userRole = "Admin";
            var instance = CreateInstance(applicationName);
            var principal = PrincipalUtils.CreatePrincipalWithApplicationRoleForUser("dorix", "morix", "dorix", applicationName, userRole);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == userRole);
        }

        private KeycloakClaimsTransformer CreateInstance(string appName = "APP")
        {
            return InstanceUtils.CreateInstanceTransformer(appName);
        }
    }
}