namespace Events.Common.Trend;

public class SearchWordDeactivated
{
    public string SearchWordId { get; set; } = default!;
    public DateTime Time { get; set; }
}