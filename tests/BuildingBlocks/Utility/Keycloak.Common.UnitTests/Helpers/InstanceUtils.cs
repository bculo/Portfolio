using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests.Helpers
{
    internal static class InstanceUtils
    {
        public static KeycloakClaimsTransformer CreateInstanceTransformer(string appName = "APP")
        {
            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakClaimOptions
            {
                ApplicationName = appName,
            });

            var logger = Mock.Of<ILogger<KeycloakClaimsTransformer>>();

            return new KeycloakClaimsTransformer(options, logger);
        }

        public static KeycloakUserInfo CreateInstanceUserInfo(ClaimsPrincipal principal)
        {
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(i => i.HttpContext.User).Returns(principal);

            return new KeycloakUserInfo(accessorMock.Object);
        }
    }
}
