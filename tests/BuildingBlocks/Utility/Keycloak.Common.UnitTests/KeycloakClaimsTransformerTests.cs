using Keycloak.Common.Constants;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Keycloak.Common.UnitTests.Generators;
using Microsoft.Extensions.Logging;
using Moq;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakClaimsTransformerTests
    {
        private readonly PrincipalManager _generator = new PrincipalManager();

        [Fact]
        public async Task TransformAsync_ShouldThrowNullReferenceException_WhenPassedValueNull()
        {
            var instance = CreateInstance();

            await Assert.ThrowsAsync<NullReferenceException>(() => instance.TransformAsync(null));
        }

        [Fact]
        public async Task TransformAsync_ShouldNotCreateRoleClaim_WhenPrincipalWithoutRolesPassed()
        {
            var principal = await _generator.CreatePrincipalForUser();
            var instance = CreateInstance();

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.DoesNotContain(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
        }

        [Fact]
        public async Task TransformAsync_ShouldCreateRoleClaim_WhenPrincipalWithSingleRealmRolePassed()
        {
            var firstUserRole = "Admin";
            string[] realmRoles = new[] { firstUserRole };
            var principal = await _generator.CreatePrincipalForUser(realmRoles: realmRoles);
            var instance = CreateInstance();

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == firstUserRole);
        }

        [Fact]
        public async Task TransformAsync_ShouldCreateMultipleRoleClaims_WhenPrincipalWithMultipleRealmRolesPassed()
        {
            var firstUserRole = "Admin";
            var secondUserRole = "PowerAdmin";
            string[] roles = new[] { firstUserRole, secondUserRole };
            var principal = await _generator.CreatePrincipalForUser(realmRoles: roles);
            var instance = CreateInstance();

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
            var firstUserRole = "Admin";
            var roles = new[] { firstUserRole };
            var instance = CreateInstance(applicationName);
            var principal = await _generator.CreatePrincipalForUser(applicationName: applicationName, appRoles: roles);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == firstUserRole);
        }

        [Fact]
        public async Task TransformAsync_ShouldCreateClientRoleClaim_WhenPrincipalWithSingleApplicationRolePassed()
        {
            var applicationName = "TEST.API";
            var clientRole = "Application";
            var roles = new[] { clientRole };
            var instance = CreateInstance(applicationName);
            var principal = await _generator.CreatePrincipalForClient(clientName: applicationName, clientRealmRole: roles);

            var result = await instance.TransformAsync(principal);

            Assert.NotNull(result);
            Assert.Contains(result.Claims, i => i.Type == KeycloakTokenConstants.ROLE);
            Assert.Contains(result.Claims, i => i.Value == clientRole);
        }

        private KeycloakClaimsTransformer CreateInstance(string appName = "APP")
        {
            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakClaimOptions
            {
                ApplicationName = appName,
            });

            var logger = Mock.Of<ILogger<KeycloakClaimsTransformer>>();

            return new KeycloakClaimsTransformer(options, logger);
        }
    }
}