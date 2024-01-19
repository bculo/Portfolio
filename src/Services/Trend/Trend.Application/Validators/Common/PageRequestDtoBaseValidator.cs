using FluentValidation;
using Dtos.Common;

namespace Trend.Application.Validators.Common
{
    public class PageRequestDtoBaseValidator : AbstractValidator<PageRequestDto>
    {
        public PageRequestDtoBaseValidator()
        {
            RuleFor(i => i.Page)
                .GreaterThan(0);
            
            RuleFor(i => i.Take)
                .GreaterThan(0);
        }
    }
}
