namespace Time.Common.Services
{
    internal class LocalDateTimeService : IDateTimeProvider
    {
        public DateTime Time => DateTime.Now;
        public DateTimeOffset TimeOffset => DateTimeOffset.Now;
    }
}
