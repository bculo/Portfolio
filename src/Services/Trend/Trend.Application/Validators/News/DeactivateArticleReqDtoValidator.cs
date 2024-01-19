using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.News;

public class DeactivateArticleReqDtoValidator : AbstractValidator<DeactivateArticleReqDto>
{
    public DeactivateArticleReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}