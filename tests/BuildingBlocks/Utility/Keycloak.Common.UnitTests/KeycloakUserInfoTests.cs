using Keycloak.Common.Services;
using Keycloak.Common.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakUserInfoTests
    {
        [Fact]
        public void GetRoles_Should_Return_Empty_Enumerable_When_ClaimPrincipal_Null()
        {
            var userInfo = CreateInstance(null);

            var roles = userInfo.GetRoles();

            Assert.NotNull(roles);
            Assert.Empty(roles);
        }

        [Fact]
        public void IsAuthenticated_Should_Return_False_When_ClaimPrincipal_Null()
        {
            var userInfo = CreateInstance(null);

            var result = userInfo.IsAuthenticated();

            Assert.False(result);
        }

        [Fact]
        public void IsAuthenticated_Should_Return_False_When_User_Not_Authenticated()
        {
            var userInfo = CreateInstance(PrincipalUtils.CreateEmptyPrincipal());

            var result = userInfo.IsAuthenticated();

            Assert.False(result);
        }

        [Fact]
        public void User_Without_Role_Test()
        {
            string firstName = "dor";
            string lastName = "mor";
            string userName = "dorix";

            var userInfo = CreateInstance(PrincipalUtils.CreatePrincipalWithoutRoleForUser(firstName, lastName, userName));

            var authenticatedResult = userInfo.IsAuthenticated();
            var userNameResult = userInfo.GetUserName();
            var emailResult = userInfo.GetEmail();
            var rolesResult = userInfo.GetRoles();

            Assert.True(authenticatedResult);
            Assert.Equal(userNameResult, userName);
            Assert.NotEmpty(emailResult);
            Assert.Equal(0, rolesResult.Count()!);
        }

        [Fact]
        public async Task User_With_Single_Role_Test()
        {
            string firstName = "dor";
            string lastName = "mor";
            string userName = "dorix";
            string userRole = "Admin";

            var principal = PrincipalUtils.CreatePrincipalWithRealmRoleForUser(firstName, lastName, userName, userRole);

            var claimTransformer = CreateInstanceUtils.CreateInstanceTransformer("TEST.API");

            var transformedPrincipal = await claimTransformer.TransformAsync(principal);

            var userInfo = CreateInstance(transformedPrincipal);

            var authenticatedResult = userInfo.IsAuthenticated();
            var userNameResult = userInfo.GetUserName();
            var emailResult = userInfo.GetEmail();
            var rolesResult = userInfo.GetRoles();
            var userInRoleResult = userInfo.IsInRole(userRole);

            Assert.True(authenticatedResult);
            Assert.Equal(userNameResult, userName);
            Assert.NotEmpty(emailResult);
            Assert.Equal(1, rolesResult.Count()!);
            Assert.True(userInRoleResult);
        }

        [Fact]
        public async Task User_With_Multiple_Roles_Test()
        {
            string firstName = "dor";
            string lastName = "mor";
            string userName = "dorix";
            string firstUserRole = "Admin";
            string secondUserRole = "User";

            var principal = PrincipalUtils.CreatePrincipalWithRealmMultipleRolesForUser(firstName, lastName, userName, firstUserRole, secondUserRole);

            var claimTransformer = CreateInstanceUtils.CreateInstanceTransformer("TEST.API");

            var transformedPrincipal = await claimTransformer.TransformAsync(principal);

            var userInfo = CreateInstance(transformedPrincipal);

            var authenticatedResult = userInfo.IsAuthenticated();
            var userNameResult = userInfo.GetUserName();
            var emailResult = userInfo.GetEmail();
            var rolesResult = userInfo.GetRoles();
            var userInFirstRoleResult = userInfo.IsInRole(firstUserRole);
            var userInSecondRoleResult = userInfo.IsInRole(secondUserRole);

            Assert.True(authenticatedResult);
            Assert.Equal(userNameResult, userName);
            Assert.NotEmpty(emailResult);
            Assert.Equal(2, rolesResult.Count()!);
            Assert.True(userInFirstRoleResult);
            Assert.True(userInSecondRoleResult);
        }

        [Fact]
        public async Task Machine_Without_Roles_Test()
        {
            string clientId = "Test.API";

            var principal = PrincipalUtils.CreatePrincipalForClientWithoutRoles(clientId);

            var claimTransformer = CreateInstanceUtils.CreateInstanceTransformer();

            var transformedPrincipal = await claimTransformer.TransformAsync(principal);

            var userInfo = CreateInstance(transformedPrincipal);

            var authenticatedResult = userInfo.IsAuthenticated();
            var clientIdResult = userInfo.GetClientId();
            var rolesResult = userInfo.GetRoles();

            Assert.True(authenticatedResult);
            Assert.Equal(clientId, clientIdResult);
            Assert.Equal(0, rolesResult.Count()!);
        }

        [Fact]
        public async Task Machine_With_Single_Role_Test()
        {
            string clientId = "Test.API";
            string role = "Application";

            var principal = PrincipalUtils.CreatePrincipalForClientWithRole(clientId, role);

            var claimTransformer = CreateInstanceUtils.CreateInstanceTransformer();

            var transformedPrincipal = await claimTransformer.TransformAsync(principal);

            var userInfo = CreateInstance(transformedPrincipal);

            var authenticatedResult = userInfo.IsAuthenticated();
            var clientIdResult = userInfo.GetClientId();
            var rolesResult = userInfo.GetRoles();

            Assert.True(authenticatedResult);
            Assert.Equal(clientId, clientIdResult);
            Assert.Equal(1, rolesResult.Count()!);
        }

        //CreatePrincipalForClientWithRole

        [Fact]
        public async Task IsAuthenticated_User_With_Multiple_Roles_Test()
        {
            string firstName = "dor";
            string lastName = "mor";
            string userName = "dorix";
            string firstUserRole = "Admin";
            string secondUserRole = "User";

            var principal = PrincipalUtils.CreatePrincipalWithRealmMultipleRolesForUser(firstName, lastName, userName, firstUserRole, secondUserRole);

            var claimTransformer = CreateInstanceUtils.CreateInstanceTransformer("TEST.API");

            var transformedPrincipal = await claimTransformer.TransformAsync(principal);

            var userInfo = CreateInstance(transformedPrincipal);

            var authenticatedResult = userInfo.IsAuthenticated();
            var userNameResult = userInfo.GetUserName();
            var emailResult = userInfo.GetEmail();
            var rolesResult = userInfo.GetRoles();
            var userInFirstRoleResult = userInfo.IsInRole(firstUserRole);
            var userInSecondRoleResult = userInfo.IsInRole(secondUserRole);

            Assert.True(authenticatedResult);
            Assert.Equal(userNameResult, userName);
            Assert.NotEmpty(emailResult);
            Assert.Equal(2, rolesResult.Count()!);
            Assert.True(userInFirstRoleResult);
            Assert.True(userInSecondRoleResult);
        }

        private KeycloakUserInfo CreateInstance(ClaimsPrincipal principal)
        {
            return CreateInstanceUtils.CreateInstanceUserInfo(principal);
        }
    }
}
