using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.SearchWord;

public class DeactivateSearchWordReqDtoValidator : AbstractValidator<DeactivateSearchWordReqDto>
{
    public DeactivateSearchWordReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}