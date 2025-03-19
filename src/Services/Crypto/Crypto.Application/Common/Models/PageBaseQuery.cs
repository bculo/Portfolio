using FluentValidation;

namespace Crypto.Application.Common.Models;

public record PageBaseQuery
{
    public int Page { get; init; }
    public int Take { get; init; }
}

public class PaginationValidator<T> : AbstractValidator<T> where T : PageBaseQuery
{
    public PaginationValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Take).GreaterThan(0);
    }
}


