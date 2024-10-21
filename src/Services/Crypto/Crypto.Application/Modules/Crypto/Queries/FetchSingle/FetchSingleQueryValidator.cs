using Crypto.Application.Common.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryValidator : AbstractValidator<FetchSingleQuery>
    {
        public FetchSingleQueryValidator()
        {
            RuleFor(i => i.Symbol)
                .MaximumLength(50)
                .MinimumLength(1)
                .NotEmpty();
        }
    }
}
