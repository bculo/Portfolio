namespace Time.Common.Services
{
    internal class UtcDateTimeService : IDateTimeProvider
    {
        public DateTime Time => DateTime.UtcNow;
        public new DateTimeOffset TimeOffset => DateTimeOffset.UtcNow;
    }
}
