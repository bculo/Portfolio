using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;
using Trend.Domain.Enums;

namespace Trend.Application.Validators.News
{
    public class FilterArticlesReqDtoValidator : AbstractValidator<FilterArticlesReqDto>
    {
        public FilterArticlesReqDtoValidator()
        {
            Include(new PageRequestDtoBaseValidator());
            
            RuleFor(i => i.Activity)
                .Must(ActiveFilter.IsValidItem)
                .WithMessage("Active filter not available.");
            
            RuleFor(i => i.Context)
                .Must(ContextType.IsValidItem)
                .WithMessage("Selected context type not available.");
        }
    }
}
