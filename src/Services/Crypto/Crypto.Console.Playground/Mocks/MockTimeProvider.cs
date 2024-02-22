using Time.Abstract.Contracts;

namespace Crypto.Console.Playground.Mocks;

public class MockTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
    public DateTime Utc => DateTime.UtcNow;
    public DateTimeOffset Offset => DateTimeOffset.UtcNow;
    public DateTimeOffset UtcOffset => DateTimeOffset.UtcNow;
} 