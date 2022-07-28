using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Keycloak.Common.UnitTests.Generators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakUserInfoTests
    {
        public const string VALID_APPLICATION_CLINED_ID = "VALID.CLIENT";
        private readonly PrincipalManager _generator;

        public KeycloakUserInfoTests()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakClaimOptions { ApplicationName = VALID_APPLICATION_CLINED_ID });
            var logger = Mock.Of<ILogger<KeycloakClaimsTransformer>>();
            _generator = new PrincipalManager(new KeycloakClaimsTransformer(options, logger));
        }

        [Fact]
        public void GetRoles_ShouldReturnEmptyEnumerable_WhenClaimPrincipalNull()
        {
            var userInfo = CreateInstance();

            var roles = userInfo.GetRoles();

            Assert.NotNull(roles);
            Assert.Empty(roles);
        }

        [Fact]
        public void IsAuthenticated_ShouldReturnFalse_WhenClaimPrincipalNull()
        {
            var userInfo = CreateInstance();

            var result = userInfo.IsAuthenticated();

            Assert.False(result);
        }

        [Fact]
        public void GetFullName_ShouldReturnNull_WhenClaimPrincipalNull()
        {
            var userInfo = CreateInstance();

            var userName = userInfo.GetFullName();

            Assert.Null(userName);
        }

        [Fact]
        public void IsAuthenticated_ShouldReturnFalse_WhenPrincipalEmpty()
        {
            var userInfo = CreateInstance(_generator.CreateEmptyPrincipal());

            var result = userInfo.IsAuthenticated();

            Assert.False(result);
        }

        [Fact]
        public void GetFullName_ShouldReturnNull_WhenPrincipalEmpty()
        {
            var userInfo = CreateInstance(_generator.CreateEmptyPrincipal());

            var userName = userInfo.GetFullName();

            Assert.Null(userName);
        }

        [Fact]
        public void GetRoles_ShouldReturnEmptyEnumerable_WhenPrincipalEmpty()
        {
            var userInfo = CreateInstance();

            var roles = userInfo.GetRoles();

            Assert.NotNull(roles);
            Assert.Empty(roles);
        }

        [Fact]
        public async Task IsAuthenticated_ShouldReturnTrue_WhenPricnipalContainsData()
        {
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", lastName: "user", username: "username");
            var userInfo = CreateInstance(principal);

            var result = userInfo.IsAuthenticated();

            Assert.True(result);
        }

        [Fact]
        public async Task GetUsername_ShouldReturnProvidedUsername_WhenPricnipalWithUsernameProvided()
        {
            string userName = "username";
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", lastName: "user", username: userName);
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetUserName();

            Assert.Equal(result, userName);
        }

        [Fact]
        public async Task GetIdentifier_ShouldReturnIdentifier_WhenPrincipalWithDataProvided()
        {
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", lastName: "user", username: "username");
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetIdentifier();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEmail_ShouldReturnEmail_WhenPrincipalWithDataProvided()
        {
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", lastName: "user", username: "username");
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetEmail();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetFullName_ShouldReturnFullName_WhenPrincipalWithDataProvided()
        {
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", lastName: "user", username: "username");
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetFullName();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetRoles_ShouldReturnDefiendRoles_WhenPrincipaWithRealmRolesDefined()
        {
            var realmRoles = new string[] { "Admin", "User" };
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", lastName: "user", username: "username", realmRoles: realmRoles);
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetRoles();

            Assert.NotEmpty(result);
            Assert.Contains(result, i => realmRoles.Contains(i));
        }

        [Fact]
        public async Task GetRoles_ShouldReturnDefiendRoles_WhenPrincipaWithApplicationRolesDefined()
        {
            var appRoles = new string[] { "Admin", "User" };
            var principal = await _generator.CreatePrincipalForUser(firstName: "user", 
                lastName: "user", 
                username: "username", 
                appRoles: appRoles, 
                applicationName: VALID_APPLICATION_CLINED_ID);
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetRoles();

            Assert.NotEmpty(result);
            Assert.Contains(result, i => appRoles.Contains(i));
        }

        [Fact]
        public async Task GetRoles_ShouldReturnEmptyRoles_WhenPrincipaWithInvalidApplicationNameDefined()
        {
            var appRoles = new string[] { "Admin", "User" };
            var principal = await _generator.CreatePrincipalForUser(firstName: "user",
                lastName: "user",
                username: "username",
                appRoles: appRoles,
                applicationName: "INVALID.APP");
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetRoles();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRoles_ShouldReturnRoles_WhenPrincipaWithRealmAndApplicationRolesDefined()
        {
            var appRoles = new string[] { "Admin", "User" };
            var realmRoles = new string[] { "Application" };
            var allRoles = appRoles.Concat(realmRoles);
            var principal = await _generator.CreatePrincipalForUser(firstName: "user",
                lastName: "user",
                username: "username",
                appRoles: appRoles,
                applicationName: "INVALID.APP",
                realmRoles: realmRoles);
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetRoles();

            Assert.NotEmpty(result);
            Assert.Contains(result, i => allRoles.Contains(i));
        }

        [Fact]
        public async Task IsAuthenticated_ShouldReturnTrue_WhenClientPrincipalProvided()
        {
            var principal = await _generator.CreatePrincipalForClient(clientName: VALID_APPLICATION_CLINED_ID);
            var userInfo = CreateInstance(principal);

            var result = userInfo.IsAuthenticated();

            Assert.True(result);
        }

        [Fact]
        public async Task GetClientId_ShouldReturnIdentifier_WhenClientPrincipalProvided()
        {
            var clientId = VALID_APPLICATION_CLINED_ID;
            var principal = await _generator.CreatePrincipalForClient(clientName: clientId);
            var userInfo = CreateInstance(principal);

            var result = userInfo.GetClientId();

            Assert.NotEmpty(result);
            Assert.Equal(result, clientId);
        }

        private KeycloakUserInfo CreateInstance(ClaimsPrincipal? principal = null)
        {
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(i => i.HttpContext.User).Returns(principal);
            return new KeycloakUserInfo(accessorMock.Object);
        }
    }
}
