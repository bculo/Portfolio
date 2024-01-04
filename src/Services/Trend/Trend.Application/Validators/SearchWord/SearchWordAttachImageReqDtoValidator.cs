using System.Collections.Frozen;
using FluentValidation;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.Validators.SearchWord;

public class SearchWordAttachImageReqDtoValidator : AbstractValidator<SearchWordAttachImageReqDto>
{
    private static readonly FrozenSet<string> _allowedContentType = new HashSet<string>
    {
        "image/jpeg",
        "image/jpg",
        "image/png"
    }.ToFrozenSet();
    
    public SearchWordAttachImageReqDtoValidator()
    {
        RuleFor(i => i.Content).NotEmpty();
        
        RuleFor(i => i.ContentType)
            .Must(IsAllowedType)
            .WithMessage("Content type must be 'jpeg' or 'png'")
            .NotEmpty();
        
        RuleFor(i => i.Name).NotEmpty();

        RuleFor(i => i.SearchWordId).NotEmpty();
    }
    
    private bool IsAllowedType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return false;
        }

        return _allowedContentType.Contains(contentType);
    }
}