using FluentValidation;

namespace Crypto.Application.Common.Models;

public record PageQuery
{
    public int Page { get; init; }
    public int Take { get; init; }
}

public class PageValidator<T> : AbstractValidator<T> where T : PageQuery
{
    public PageValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Take).GreaterThan(0);
    }
}


