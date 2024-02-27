using Events.Common.User;
using FluentValidation;
using Keycloak.Common.Models;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
using User.Application.Common.Exceptions;

namespace User.Application.Features;

public record VerifyUserDto : IRequest
{
    public string UserName { get; set; } = default!;
}

public class VerifyUserDtoValidator : AbstractValidator<VerifyUserDto>
{
    public VerifyUserDtoValidator()
    {
        RuleFor(i => i.UserName)
            .NotEmpty();
    }
}

public class VerifyUserDtoHandler : IRequestHandler<VerifyUserDto>
{
    private readonly ILogger<VerifyUserDtoHandler> _logger;
    private readonly IPublishEndpoint _publish;
    private readonly KeycloakAdminApiOptions _config;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IUsersApi _userClient;

    public VerifyUserDtoHandler(IUsersApi userClient,
        IPublishEndpoint publish,
        IDateTimeProvider timeProvider,
        IOptions<KeycloakAdminApiOptions> config, 
        ILogger<VerifyUserDtoHandler> handler)
    {
        _userClient = userClient;
        _publish = publish;
        _timeProvider = timeProvider;
        _logger = handler;
        _config = config.Value;
    }
    
    public async Task Handle(VerifyUserDto request, CancellationToken token)
    {
        var keycloakUser = await FetchKeycloakUserByUniqueUserName(request.UserName);
        
        var keycloakUpdateRequest = new UserRepresentation
        {
            Attributes = new Dictionary<string, ICollection<object>>
            {
                { "verified", new List<object> { "true" } }
            } 
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
