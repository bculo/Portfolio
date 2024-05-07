using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQueryValidator : AbstractValidator<FetchPageQuery>
    {
        public FetchPageQueryValidator()
        {
            RuleFor(i => i.Take).GreaterThan(0);
            RuleFor(i => i.Page).GreaterThan(0);
        }
    }
}
