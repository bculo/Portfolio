using Auth0.Abstract.Contracts;
using FluentAssertions;
using NSubstitute;
using Trend.API.Services;
using Trend.Domain.Exceptions;

namespace Trend.UnitTests.API;

public class UserServiceTests
{
    private readonly IAuth0AccessTokenReader _reader = Substitute.For<IAuth0AccessTokenReader>();

    [Fact]
    public void UserId_ShouldThrowException_WhenUserIdentifierNotAvailable()
    {
        _reader.GetIdentifier().Returns(Guid.Empty);

        var service = new UserService(_reader);

        Assert.Throws<TrendAppAuthenticationException>(() => service.UserId);
    }
    
    [Fact]
    public void UserId_ShouldReturnUserIdentifier_WhenUserIdentifierAvailable()
    {
        var userId = Guid.NewGuid();
        _reader.GetIdentifier().Returns(userId);

        var service = new UserService(_reader);

        var serviceUserId = service.UserId;

        serviceUserId.Should().Be(userId);
    }
}