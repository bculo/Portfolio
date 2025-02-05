using FluentValidation;
using Queryable.Common.Models;

namespace Stock.Application.Common.Models;

public record PageRequestDto : PageQuery;

public class PageRequestDtoValidator : AbstractValidator<PageRequestDto>
{
    public PageRequestDtoValidator()
    {
        RuleFor(i => i.Page)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(i => i.Take)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}

