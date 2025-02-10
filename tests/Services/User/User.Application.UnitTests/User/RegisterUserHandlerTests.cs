using System.Net;
using AutoFixture;
using Events.Common.User;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Keycloak.Common.Services.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using NSubstitute;
using Refit;
using Time.Abstract.Contracts;
using User.Application.Common.Exceptions;
using User.Application.Features;

namespace User.Application.UnitTests.User;

public class RegisterUserHandlerTests
{
    private const int YearGap = -30;
    private readonly Fixture _fixture = new();
    private readonly IUsersApi _usersApi = Substitute.For<IUsersApi>();
    private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();
    private readonly IDateTimeProvider _timeProvider = Substitute.For<IDateTimeProvider>();
    private readonly IOptions<KeycloakAdminApiOptions> _options;
    
    public RegisterUserHandlerTests()
    {
        _timeProvider.Time.Returns(DateTime.UtcNow);
        _options = Options.Create(_fixture.Create<KeycloakAdminApiOptions>());
    }

    private RegisterUserHandler GetHandler()
    {
        return new RegisterUserHandler(_usersApi, _publishEndpoint, _timeProvider, _options);
    }

    private RegisterUserDto GetValidDto()
    {
        return new RegisterUserDto()
        {
            UserName = "UserName",
            FirstName = "FirstName",
            LastName = "LastName",
            Password = "Password",
            Born = DateTime.UtcNow.AddYears(YearGap),
            Email = "test@mail.com"
        };
    }
    
    [Fact]
    public async Task ShouldPublishNewUserRegisteredEvent_WhenUserIsSuccessfullyCreatedViaApi()
    {
        _usersApi.PostUsers(Arg.Any<string>(), Arg.Any<UserRepresentation>()).Returns(
            new ApiResponse<string>(new HttpResponseMessage(), " ", new RefitSettings(), default));
        
        var handler = GetHandler();
        var dto = GetValidDto();

        await handler.Handle(dto, default);

        await _publishEndpoint.Received(1).Publish(Arg.Any<NewUserRegistered>());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenConflictOccurs()
    {
        _usersApi.PostUsers(Arg.Any<string>(), Arg.Any<UserRepresentation>()).Returns(
            new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent("errorMessage")
            }, "", new RefitSettings(), default));
        
        var handler = GetHandler();
        var dto = GetValidDto();
        
        await Assert.ThrowsAsync<PortfolioUserCoreException>(async () => await handler.Handle(dto, default));
    }
}