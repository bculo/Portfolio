using FluentAssertions;
using Keycloak.Common.Utils;

namespace Keycloak.Common.UnitTests;

public class UriUtilsTests
{
    [Theory]
    [InlineData("http://localhost:8080/auth/realms/PortfolioRealm", "protocol/openid-connect/token")]
    [InlineData("http://localhost:8080/auth/realms/PortfolioRealm", "test")]
    public void JoinUriSegments_ShouldReturnStringCombination_WhenValidArgumentsProvided(string uri, string segments)
    {
        var result = UriUtils.JoinUriSegments(uri, segments);
        
        result.Should().Be($"{uri}/{segments}");
    }
}