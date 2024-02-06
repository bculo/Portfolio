namespace Stock.Application.Common.Extensions;

public static class TimeExtensions
{
    public static DateTime WithoutSeconds(this DateTime time)
    {
        return time.Date + new TimeSpan(time.TimeOfDay.Hours, time.TimeOfDay.Minutes, 0);
    }
}