using FluentValidation;
using Dtos.Common;

namespace Trend.Application.Validators.Common
{
    public class PageRequestDtoBaseValidator<T> : AbstractValidator<T> where T : PageRequestDto
    {
        public PageRequestDtoBaseValidator()
        {
            RuleFor(i => i.Page)
                .GreaterThan(0);
            
            RuleFor(i => i.Take)
                .GreaterThan(0);
        }
    }

    public class PageRequestDtoValidator : PageRequestDtoBaseValidator<PageRequestDto>
    {
        public PageRequestDtoValidator() : base()
        {

        }
    }
}
