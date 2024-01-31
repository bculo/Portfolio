using Microsoft.AspNetCore.Mvc.RazorPages;
using Stock.Application.Common.Models;
using Stock.Core.Models.Common;

namespace Stock.Application.Common.Extensions;

public static class PageQueryExtensions
{
    public static PageResultDto<TDto> MapToDto<TModel, TDto>(
        this PageModel<TModel> page,
        Func<TModel, TDto> projectionFunc)
    {
        var dtoPageItems = page.Items.Select(projectionFunc);
        return new PageResultDto<TDto>(page.TotalCount, dtoPageItems);
    }
}