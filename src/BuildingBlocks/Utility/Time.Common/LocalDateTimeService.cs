using Time.Abstract.Contracts;

namespace Time.Common
{
    public class LocalDateTimeService : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
