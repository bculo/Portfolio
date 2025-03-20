using FluentValidation;

namespace Crypto.Application.Common.Models;

public record TakeQuery
{
    public int Take { get; init; }
}

public class TakeValidator<T> : AbstractValidator<T> where T : TakeQuery
{
    public TakeValidator()
    {
        RuleFor(x => x.Take).GreaterThan(0);
        RuleFor(x => x.Take).LessThan(1000);
    }
}