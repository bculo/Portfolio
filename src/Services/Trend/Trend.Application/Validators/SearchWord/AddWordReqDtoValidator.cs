using FluentValidation;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

namespace Trend.Application.Validators.SearchWord;

public class AddWordReqDtoValidator : AbstractValidator<AddWordReqDto>
{
    private readonly ISearchWordRepository _wordRepository;
    
    public AddWordReqDtoValidator(ISearchWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
        
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(i => i.SearchEngine)
            .Must(SearchEngine.IsValidItem)
            .WithMessage("Selected search engine not available.")
            .Must(SearchEngine.IsValidForNewSearchWord)
            .WithMessage("Selected search engine type not available");
            
        
        RuleFor(i => i.ContextType)
            .Must(ContextType.IsValidItem)
            .WithMessage("Selected context type not available.")
            .Must(ContextType.IsValidForNewSearchWord)
            .WithMessage("Selected context type not valid.");

        RuleFor(i => i.SearchWord)
            .NotEmpty()
            .MinimumLength(2)
            .MustAsync(BeUnique)
            .When(dto => SearchEngine.IsValidForNewSearchWord(dto.SearchEngine))
            .WithMessage("Search word is not unique");
    }

    private async Task<bool> BeUnique(AddWordReqDto item, string searchWord, CancellationToken token)
    {
        return !await _wordRepository.IsDuplicate(searchWord, item.SearchEngine, token);
    }
}