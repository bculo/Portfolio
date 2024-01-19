using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.SearchWord;

public class ActivateSearchWordReqDtoValidator : AbstractValidator<ActivateSearchWordReqDto>
{
    public ActivateSearchWordReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}