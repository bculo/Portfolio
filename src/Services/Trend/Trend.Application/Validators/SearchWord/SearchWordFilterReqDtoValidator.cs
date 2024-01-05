using FluentValidation;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.Validators.Common;
using Trend.Domain.Enums;

namespace Trend.Application.Validators.SearchWord;

public class SearchWordFilterReqDtoValidator : PageRequestDtoBaseValidator<SearchWordFilterReqDto>
{
    public SearchWordFilterReqDtoValidator()
    {
        RuleFor(i => i.SearchEngine)
            .Must(SearchEngine.IsValidItem)
            .WithMessage("Selected search engine type not available");
        
        RuleFor(i => i.ContextType)
            .Must(ContextType.IsValidItem)
            .WithMessage("Selected context type not available.");
        
        RuleFor(i => i.Sort)
            .Must(SortType.IsValidItem)
            .WithMessage("Sort type not available.");
        
        RuleFor(i => i.Active)
            .Must(ActiveFilter.IsValidItem)
            .WithMessage("Active filter not available.");

        RuleFor(i => i.Query)
            .MaximumLength(50);
    }
}