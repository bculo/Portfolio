namespace Time.Common;

public class IDateTimeProvider
{
    public DateTime Time { get; }
    public DateTimeOffset TimeOffset { get; }
}