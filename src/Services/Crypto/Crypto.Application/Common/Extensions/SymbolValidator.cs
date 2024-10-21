using Crypto.Application.Common.Constants;
using FluentValidation;

namespace Crypto.Application.Common.Extensions;

public static class SymbolValidator
{
    public static IRuleBuilder<T, string> WithSymbolRule<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(50)
            .Matches(RegexConstants.Symbol)
            .NotEmpty();
    }
}