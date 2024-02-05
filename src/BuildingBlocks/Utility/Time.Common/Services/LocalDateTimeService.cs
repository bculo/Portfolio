using Time.Abstract.Contracts;

namespace Time.Common.Services
{
    internal class LocalDateTimeService : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime Utc => DateTime.UtcNow;
    }
}
