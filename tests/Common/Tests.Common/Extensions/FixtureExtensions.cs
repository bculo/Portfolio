using AutoFixture;

namespace Tests.Common.Extensions;

public static class FixtureExtensions
{
    public static Fixture Configure(this Fixture fixture)
    {
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}