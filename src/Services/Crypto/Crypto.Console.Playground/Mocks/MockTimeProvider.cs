using Time.Common;

namespace Crypto.Console.Playground.Mocks;

public class MockTimeProvider : IDateTimeProvider
{
    public DateTime Time => DateTime.UtcNow;
    public DateTimeOffset TimeOffset => DateTimeOffset.UtcNow;
} 