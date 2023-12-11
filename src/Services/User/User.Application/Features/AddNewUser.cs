using Events.Common.User;
using FluentValidation;
using Keycloak.Common.Models;
using Keycloak.Common.Options;
using Keycloak.Common.Refit;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
using User.Application.Entities;
using User.Application.Persistence;

namespace User.Application.Features;

public class AddNewUserDto : IRequest
{
    public DateTime Born { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AddNewUserDtoValidator : AbstractValidator<AddNewUserDto>
{
    private readonly UserDbContext _context;
    private readonly IDateTimeProvider _timeProvider;

    public AddNewUserDtoValidator(IDateTimeProvider timeProvider, UserDbContext context)
    {
        _timeProvider = timeProvider;
        _context = context;

        RuleFor(i => i.UserName)
            .MustAsync(IsUnique)
            .WithMessage("Given username is already taken.")
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
    
    private async Task<bool> IsUnique(string userName, CancellationToken token)
    {
        return await _context.Users.AllAsync(i => i.UserName != userName, token);
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

public class AddNewUserHandler : IRequestHandler<AddNewUserDto>
{
    private readonly UserDbContext _context;
    private readonly IPublishEndpoint _publish;
    private readonly KeycloakAdminApiOptions _config;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IUsersApi _userClient;

    public AddNewUserHandler(UserDbContext context,
        IUsersApi userClient,
        IPublishEndpoint publish,
        IDateTimeProvider timeProvider,
        IOptions<KeycloakAdminApiOptions> config)
    {
        _context = context;
        _userClient = userClient;
        _publish = publish;
        _timeProvider = timeProvider;
        _config = config.Value;
    }
    
    public async Task Handle(AddNewUserDto request, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            var entity = MapToEntityModel(request);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync(token).ConfigureAwait(false);
                
            var keyCloakModel = MapToKeycloakModel(request);
            var response = await _userClient.PostUsers(_config.Realm, keyCloakModel)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // DO SOMETHING
            }
                
            await transaction.CommitAsync(token);
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync(token);

            await _publish.Publish(new DeleteKeycloakUser
            {
                UserName = request.UserName,
                Time = _timeProvider.Now
            }, token);
                
            throw;
        }

        await _publish.Publish(new NewUserRegistered
        {
            UserName = request.UserName,
            Time = _timeProvider.Now
        }, token);
    }
    
    private UserRepresentation MapToKeycloakModel(AddNewUserDto userDto)
    {
        return new UserRepresentation
        {
            Enabled = false,
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
    
    private PortfolioUser MapToEntityModel(AddNewUserDto userDto)
    {
        return new PortfolioUser
        {
            BornOn = userDto.Born,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            UserName = userDto.UserName,
        };
    }
}