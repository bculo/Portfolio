using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.SearchBySymbol
{
    public class SearchBySymbolQueryValidator : AbstractValidator<SearchBySymbolQuery>
    {
        public SearchBySymbolQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .MaximumLength(20)
                .NotNull();
        }
    }
}
