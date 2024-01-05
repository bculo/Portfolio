namespace Trend.Application.Interfaces.Models.Repositories
{
    public class PageReqQuery
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public int Skip => (Page - 1) * Take;
    }
}
