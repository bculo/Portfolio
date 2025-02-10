using Time.Abstract.Contracts;

namespace Crypto.Console.Playground.Mocks;

public class MockTimeProvider : IDateTimeProvider
{
    public DateTime Time => DateTime.UtcNow;
    public DateTime Utc => DateTime.UtcNow;
    public DateTimeOffset TimeOffset => DateTimeOffset.UtcNow;
    public DateTimeOffset UtcOffset => DateTimeOffset.UtcNow;
} 