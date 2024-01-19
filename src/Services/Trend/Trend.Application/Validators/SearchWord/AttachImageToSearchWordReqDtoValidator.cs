using System.Collections.Frozen;
using FluentValidation;
using Trend.Application.Interfaces.Models;

namespace Trend.Application.Validators.SearchWord;

public class AttachImageToSearchWordReqDtoValidator : AbstractValidator<AttachImageToSearchWordReqDto>
{
    private static readonly FrozenSet<string> _allowedContentType = new HashSet<string>
    {
        "image/jpeg",
        "image/png"
    }.ToFrozenSet();
    
    public AttachImageToSearchWordReqDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(i => i.Content)
            .NotEmpty();

        RuleFor(i => i.ContentType)
            .NotEmpty()
            .Must(IsAllowedType)
            .WithMessage("Content type must be 'jpeg' or 'png'");

        RuleFor(i => i.Name)
            .NotEmpty();

        RuleFor(i => i.Id)
            .NotEmpty();
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