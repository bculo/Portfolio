using Time.Abstract.Contracts;

namespace Time.Common
{
    public class UtcDateTimeService : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
