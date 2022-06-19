namespace Trend.Domain.Interfaces
{
    public interface IDocumentRoot
    {
        string Id { get; set; }
        public DateTime Created { get; set; }
    }
}
