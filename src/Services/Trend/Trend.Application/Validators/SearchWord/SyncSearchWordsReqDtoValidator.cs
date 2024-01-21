using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.SearchWord;

public class SyncSearchWordsReqDtoValidator : AbstractValidator<SyncSearchWordsReqDto>
{
    public SyncSearchWordsReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}