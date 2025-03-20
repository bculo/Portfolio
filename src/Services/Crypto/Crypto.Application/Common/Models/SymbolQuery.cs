using Crypto.Application.Common.Extensions;
using FluentValidation;

namespace Crypto.Application.Common.Models;

public record SymbolQuery
{
    public string Symbol { get; init; } = null!;
}

public class SymbolQueryValidator<T> : AbstractValidator<T> where T : SymbolQuery
{
    public SymbolQueryValidator()
    {
        RuleFor(x => x.Symbol).WithSymbolRule();
    }
}

public record NullableSymbolQuery
{
    public string? Symbol { get; init; }
}

public class NullableSymbolQueryValidator<T> : AbstractValidator<T> where T : NullableSymbolQuery
{
    public NullableSymbolQueryValidator()
    {
        RuleFor(x => x.Symbol!)
            .WithSymbolRule()
            .When(item => !string.IsNullOrWhiteSpace(item.Symbol));
    }
}