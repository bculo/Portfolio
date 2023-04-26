using Crypto.Application.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryValidator : AbstractValidator<FetchSingleQuery>
    {
        public FetchSingleQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.SYMBOL)
                .MinimumLength(1)
                .NotEmpty();
        }
    }
}
