using Time.Abstract.Contracts;

namespace Time.Common.Services
{
    internal class UtcDateTimeService : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
