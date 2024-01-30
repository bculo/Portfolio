using System.Text.RegularExpressions;
using FluentValidation;

namespace Stock.Application.Common.Extensions;

public static class ValidationRegex
{
    public static readonly Regex SymbolRegex = new ("^[a-zA-Z]{1,10}$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));
}


public static class StockFluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MatchesStockSymbol<T>(
        this IRuleBuilder<T, string> builder)
    {
        return builder.Matches(ValidationRegex.SymbolRegex);
    }
    
    public static IRuleBuilderOptions<T, string> MatchesStockSymbolWhen<T>(
        this IRuleBuilder<T, string> builder,
        Func<T, bool> predicate)
    {
        return builder.Matches(ValidationRegex.SymbolRegex)
            .When(predicate);
    }
}