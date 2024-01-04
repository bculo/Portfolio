namespace Trend.Application.Interfaces.Models.Repositories
{
    public class PageReqQuery<T> 
    {
        public int Page { get; private set; }
        public int Take { get; private set; }
        public T Search { get; private set; }
        public int Skip => Page - 1;

        public PageReqQuery(int page, int take, T search)
        {
            Page = page;
            Take = take;
            Search = search;
        }
    }

    public class PageReqQuery : PageReqQuery<object>
    {
        public PageReqQuery(int page, int take) : base(page, take, null) 
        {

        }
    }
}
