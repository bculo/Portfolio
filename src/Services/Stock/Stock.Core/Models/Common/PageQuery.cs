using Stock.Core.Models.Base;

namespace Stock.Core.Models.Common;

public class PageQuery : IQuery
{
    public int Page { get; }
    public int Take { get; }

    public int Skip => (Page - 1) * Take;

    public PageQuery(int page, int take)
    {
        Page = page;
        Take = take;
    }
}