namespace Trend.Application.Interfaces.Models.Repositories
{
    public class PageResQuery<T> where T : notnull
    {
        public long Count { get; private set; }
        public List<T> Items { get; private set; }

        public PageResQuery(long count, List<T> items)
        {
            Count = count;
            Items = items;
        }
    }
}
