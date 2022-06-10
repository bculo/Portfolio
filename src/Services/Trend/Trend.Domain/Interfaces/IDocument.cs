namespace Trend.Domain.Interfaces
{
    public interface IDocument
    {
        string Id { get; set; }
        public DateTime Created { get; set; }
    }
}
