using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.Sync;

public class GetSyncStatusReqDtoValidator : AbstractValidator<GetSyncStatusReqDto>
{
    public GetSyncStatusReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}