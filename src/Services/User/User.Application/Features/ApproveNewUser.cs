using Events.Common.User;
using FluentValidation;
using Keycloak.Common.Models;
using Keycloak.Common.Options;
using Keycloak.Common.Refit;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
using User.Application.Common.Exceptions;
using User.Application.Persistence;

namespace User.Application.Features;

public record ApproveNewUserDto : IRequest
{
    public string UserName { get; set; }
}

public class ApproveNewUserDtoValidator : AbstractValidator<ApproveNewUserDto>
{
    public ApproveNewUserDtoValidator()
    {
        RuleFor(i => i.UserName).NotEmpty();
    }
}

public class ApproveNewUserHandler : IRequestHandler<ApproveNewUserDto>
{
    private readonly IPublishEndpoint _publish;
    private readonly KeycloakAdminApiOptions _config;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IUsersApi _userClient;

    public ApproveNewUserHandler(IUsersApi userClient,
        IPublishEndpoint publish,
        IDateTimeProvider timeProvider,
        IOptions<KeycloakAdminApiOptions> config)
    {
        _userClient = userClient;
        _publish = publish;
        _timeProvider = timeProvider;
        _config = config.Value;
    }
    
    public async Task Handle(ApproveNewUserDto request, CancellationToken token)
    {
        var keycloakUser = await FetchKeycloakUserByUniqueUserName(request.UserName);
        
        var keycloakUpdateRequest = new UserRepresentation
        {
            Enabled = true,
        };

        await UpdateKeyCloakUser(keycloakUpdateRequest, keycloakUser.Id);
        
        await _publish.Publish(new UserApproved
        {
            UserName = keycloakUser.Username,
            ExternalId = Guid.Parse(keycloakUser.Id),
            ApprovedOn = _timeProvider.Now
        }, token);
    }
    
    private async Task<UserRepresentation> FetchKeycloakUserByUniqueUserName(string userName)
    {
        var keycloakSearchResult = await _userClient.GetUsersByRealm(_config.Realm, 
            exact: "true",
            username: userName).ConfigureAwait(false);
            
        if (!keycloakSearchResult.Any())
        {
            throw new PortfolioUserNotFoundException("User not found");
        }

        return keycloakSearchResult.First();
    }
    
    private async Task UpdateKeyCloakUser(UserRepresentation keycloakUpdateRequest, string userId)
    {
        var response = await _userClient.PutUser(_config.Realm, userId, keycloakUpdateRequest)
            .ConfigureAwait(false);
        
        if(!response.IsSuccessStatusCode)
        {
            throw new PortfolioUserCoreException(
                "Couldn't update existing Keycloak user via admin API",
                "An error occurred. Please try again later.");
        }
    }
}
