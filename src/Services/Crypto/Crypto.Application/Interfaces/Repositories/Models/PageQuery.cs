namespace Crypto.Application.Interfaces.Repositories.Models;

public class PageQuery
{
    public int Page { get; }
    public int Take { get; }

    public int Skip => (Page - 1) * Take;

    public PageQuery(int page, int take)
    {
        Page = page;
        Take = take;
    }
    
    public static implicit operator (int, int)(PageQuery context) => (context.Skip, context.Take);
}