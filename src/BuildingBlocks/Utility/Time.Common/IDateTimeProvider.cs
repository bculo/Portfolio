namespace Time.Common;

public interface IDateTimeProvider
{
    DateTime Time { get; } 
    DateTimeOffset TimeOffset { get; }
}