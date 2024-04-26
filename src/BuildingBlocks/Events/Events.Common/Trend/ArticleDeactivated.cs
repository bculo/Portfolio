namespace Events.Common.Trend;

public class ArticleDeactivated
{
    public string ArticleId { get; set; } = default!;
    public DateTime Time { get; set; }
}