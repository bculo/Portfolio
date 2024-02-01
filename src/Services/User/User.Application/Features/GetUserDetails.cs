using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using User.Application.Common.Exceptions;
using User.Application.Entities;
using User.Application.Persistence;

namespace User.Application.Features;

public record GetUserDetailsDto : IRequest<GetUserDetailsResponseDto>
{
    public Guid UserId { get; set; }
}

public record GetUserDetailsResponseDto
{
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

public class GetUserDetailsDtoValidator : AbstractValidator<GetUserDetailsDto>
{
    public GetUserDetailsDtoValidator()
    {
        RuleFor(i => i.UserId)
            .NotEmpty();
    }
}

public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsDto, GetUserDetailsResponseDto>
{
    private readonly UserDbContext _context;

    public GetUserDetailsHandler(UserDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetUserDetailsResponseDto> Handle(GetUserDetailsDto request, CancellationToken cancellationToken)
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