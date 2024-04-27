namespace Events.Common.Trend;

public class SearchWordActivated
{
    public string SearchWordId { get; set; } = default!;
    public DateTime Time { get; set; }
}