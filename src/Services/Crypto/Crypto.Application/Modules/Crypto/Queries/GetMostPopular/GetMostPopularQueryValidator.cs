using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryValidator : AbstractValidator<GetMostPopularQuery>
    {
        public GetMostPopularQueryValidator()
        {
            RuleFor(i => i.Take).GreaterThan(0);
        }
    }
}
