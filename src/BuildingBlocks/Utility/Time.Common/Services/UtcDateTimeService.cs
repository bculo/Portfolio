using Time.Abstract.Contracts;

namespace Time.Common.Services
{
    internal class UtcDateTimeService : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTime Utc => DateTime.UtcNow;

        public DateTimeOffset Offset => DateTimeOffset.Now;
        public DateTimeOffset UtcOffset => DateTimeOffset.UtcNow;
    }
}
