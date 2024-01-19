using FluentValidation;
using Trend.Application.Interfaces.Models;

namespace Trend.Application.Validators.News;

public class ActivateArticleReqDtoValidator : AbstractValidator<ActivateArticleReqDto>
{
    public ActivateArticleReqDtoValidator()
    {
        RuleFor(i => i.ArticleId).NotEmpty();
    }
}