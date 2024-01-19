using Dtos.Common;
using FluentValidation;

namespace Trend.Application.Validators.Common;

public class GetItemByStringIdReqDtoBaseValidator : AbstractValidator<GetItemByStringIdReqDto>
{
    public GetItemByStringIdReqDtoBaseValidator()
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}