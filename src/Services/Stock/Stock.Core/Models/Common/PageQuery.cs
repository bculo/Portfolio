using Stock.Core.Models.Base;

namespace Stock.Core.Models.Common;

public class PageQuery(int page, int take) : IQuery
{
    public int Page { get; } = page;
    public int Take { get; } = take;

    public int Skip => (Page - 1) * Take;

    public static implicit operator (int, int)(PageQuery context) => (context.Skip, context.Take);
}