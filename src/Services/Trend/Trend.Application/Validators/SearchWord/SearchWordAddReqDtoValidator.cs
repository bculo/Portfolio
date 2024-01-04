using FluentValidation;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;

namespace Trend.Application.Validators.SearchWord;

public class SearchWordAddReqDtoValidator : AbstractValidator<SearchWordAddReqDto>
{
    public SearchWordAddReqDtoValidator()
    {
        RuleFor(i => i.SearchWord).MinimumLength(2).NotEmpty();

        RuleFor(i => i.SearchEngine).Must(engine => 
                Enum.GetValues<SearchEngine>()
                    .Cast<int>()
                    .Contains(engine))
            .WithMessage("Selected search engine type not available");

        RuleFor(i => i.ContextType).Must(type => 
                Enum.GetValues<ContextType>()
                    .Cast<int>()
                    .Contains(type))
            .WithMessage("Selected context type not available");
    }
}