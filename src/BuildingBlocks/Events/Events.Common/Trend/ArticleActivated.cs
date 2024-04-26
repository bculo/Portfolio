namespace Events.Common.Trend;

public class ArticleActivated
{
    public string ArticleId { get; set; } = default!;
    public DateTime Time { get; set; }
}