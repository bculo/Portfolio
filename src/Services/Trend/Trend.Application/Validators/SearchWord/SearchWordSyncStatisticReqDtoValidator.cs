using FluentValidation;
using Trend.Application.Interfaces.Models;
using Trend.Application.Validators.Common;

namespace Trend.Application.Validators.SearchWord;

public class SearchWordSyncStatisticReqDtoValidator : AbstractValidator<SearchWordSyncStatisticReqDto>
{
    public SearchWordSyncStatisticReqDtoValidator()
    {
        Include(new GetItemByStringIdReqDtoBaseValidator());
    }
}