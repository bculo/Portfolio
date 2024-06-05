using System.Net;
using Events.Common.User;
using FluentValidation;
using Keycloak.Common.Models;
using Keycloak.Common.Models.Errors;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Time.Abstract.Contracts;
using User.Application.Common.Exceptions;

namespace User.Application.Features;

public record RegisterUserDto : IRequest
{
    public DateTime Born { get; init; }
    public string UserName { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private readonly IDateTimeProvider _timeProvider;

    public RegisterUserDtoValidator(IDateTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;

        RuleFor(i => i.UserName)
            .MinimumLength(5)
            .MaximumLength(50)
            .NotEmpty();

        RuleFor(i => i.FirstName)
            .MaximumLength(128)
            .NotEmpty();

        RuleFor(i => i.LastName)
            .MaximumLength(128)
            .NotEmpty();

        RuleFor(i => i.Born)
            .Must(IsAdultPerson)
            .WithMessage("Person must be at least 18 years old to use this application.")
            .NotEmpty();

        RuleFor(i => i.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(i => i.Password)
            .MinimumLength(4)
            .NotEmpty();
    }
    
    private bool IsAdultPerson(DateTime bornOn)
    {
        if ((_timeProvider.Now.Year - bornOn.Year) < 18)
        {
            return false;
        }

        return true;
    }
}

public class RegisterUserHandler : IRequestHandler<RegisterUserDto>
{
    private readonly IPublishEndpoint _publish;
    private readonly KeycloakAdminApiOptions _config;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IUsersApi _userClient;

    public RegisterUserHandler(
        IUsersApi userClient,
        IPublishEndpoint publish,
        IDateTimeProvider timeProvider,
        IOptions<KeycloakAdminApiOptions> config)
    {
        _userClient = userClient;
        _publish = publish;
        _timeProvider = timeProvider;
        _config = config.Value;
    }
    
    public async Task Handle(RegisterUserDto request, CancellationToken token)
    {
        var keyCloakModel = MapToKeycloakModel(request);
        var response = await _userClient.PostUsers(_config.Realm, keyCloakModel)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            await _publish.Publish(new NewUserRegistered
            {
                UserName = request.UserName,
                Time = _timeProvider.Now
            }, token);
            return;
        }

        var errorContent = response.Error?.Content ?? string.Empty;
        if (response.StatusCode == HttpStatusCode.Conflict && errorContent.Contains("errorMessage"))
        {
            var error = JsonConvert.DeserializeObject<AddNewUserError>(errorContent);
            throw new PortfolioUserCoreException(error?.ErrorMessage, error?.ErrorMessage);
        }

        throw new PortfolioUserCoreException("An problem occured. Try again later",
            "An problem occured. Try again later");
    }
    
    private UserRepresentation MapToKeycloakModel(RegisterUserDto userDto)
    {
        return new UserRepresentation
        {
            Enabled = true,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Username = userDto.UserName,
            Id = Guid.NewGuid().ToString(),
            RealmRoles = new[] { "User" },
            Credentials = new[]
            {
                new CredentialRepresentation
                {
                    Temporary = false,
                    Type = "password",
                    Value = userDto.Password
                }
            },
            Email = userDto.Email,
        };
    }
}