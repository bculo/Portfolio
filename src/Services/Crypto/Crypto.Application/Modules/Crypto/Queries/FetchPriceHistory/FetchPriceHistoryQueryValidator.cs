using Crypto.Application.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryValidator : AbstractValidator<FetchPriceHistoryQuery>
    {
        public FetchPriceHistoryQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.SYMBOL)
                .NotEmpty();
        }   
    }
}
