using Crypto.Application.Common.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryValidator : AbstractValidator<FetchSingleQuery>
    {
        public FetchSingleQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .Matches(RegexConstants.Symbol)
                .MinimumLength(1)
                .NotEmpty();
        }
    }
}
