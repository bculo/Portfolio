using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.News;

public class ActivateArticleReqDtoValidator : AbstractValidator<ActivateArticleReqDto>
{
    public ActivateArticleReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}