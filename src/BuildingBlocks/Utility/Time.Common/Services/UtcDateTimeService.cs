namespace Time.Common.Services
{
    internal class UtcDateTimeService : IDateTimeProvider
    {
        public DateTime Time => DateTime.UtcNow;
        public DateTimeOffset TimeOffset => DateTimeOffset.UtcNow;
    }
}
