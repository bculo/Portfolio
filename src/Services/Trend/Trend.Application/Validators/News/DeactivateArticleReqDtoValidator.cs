using FluentValidation;
using Trend.Application.Interfaces.Models;

namespace Trend.Application.Validators.News;

public class DeactivateArticleReqDtoValidator : AbstractValidator<DeactivateArticleReqDto>
{
    public DeactivateArticleReqDtoValidator()
    {
        RuleFor(i => i.ArticleId).NotEmpty();
    }
}