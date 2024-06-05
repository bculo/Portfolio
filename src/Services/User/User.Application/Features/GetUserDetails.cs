using FluentValidation;
using MediatR;
using User.Application.Entities;
using User.Application.Persistence;

namespace User.Application.Features;

public record GetUserDetailsDto : IRequest<GetUserDetailsResponseDto>
{
    public Guid UserId { get; init; }
}

public record GetUserDetailsResponseDto
{
    public string UserName { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
}

public class GetUserDetailsDtoValidator : AbstractValidator<GetUserDetailsDto>
{
    public GetUserDetailsDtoValidator()
    {
        RuleFor(i => i.UserId)
            .NotEmpty();
    }
}

public class GetUserDetailsHandler(UserDbContext context)
    : IRequestHandler<GetUserDetailsDto, GetUserDetailsResponseDto>
{
    private readonly UserDbContext _context = context;

    public Task<GetUserDetailsResponseDto> Handle(GetUserDetailsDto request, CancellationToken cancellationToken)
    {
        return null;
    }
    
    private GetUserDetailsResponseDto MapToDto(PortfolioUser user)
    {
        return new GetUserDetailsResponseDto
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
    }
}