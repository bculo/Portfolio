using Time.Abstract.Contracts;

namespace Time.Common.Services
{
    internal class LocalDateTimeService : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime Utc => DateTime.UtcNow;
        public DateTimeOffset Offset => DateTimeOffset.Now;
        public DateTimeOffset UtcOffset => DateTimeOffset.UtcNow;
    }
}
