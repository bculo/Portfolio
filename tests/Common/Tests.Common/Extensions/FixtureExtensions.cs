using AutoFixture;

namespace Tests.Common.Extensions;

public static class FixtureExtensions
{
    public static Fixture GetFixture()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}