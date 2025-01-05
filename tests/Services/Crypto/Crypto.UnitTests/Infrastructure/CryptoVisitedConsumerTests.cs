using Crypto.Infrastructure.Consumers;
using Events.Common.Crypto;
using FluentAssertions;

namespace Crypto.UnitTests.Infrastructure;

public class CryptoVisitedConsumerTests(MassTransitFixture fixture) : IClassFixture<MassTransitFixture>
{
    [Fact]
    public async Task ShouldReceiveEvent_WhenPublished()
    {
        var harness = fixture.GetTestHarness();
        await harness.Start();
        
        await harness.Bus.Publish(new Visited
        {
            CryptoId = Guid.NewGuid(),
            Symbol = "BTC"
        });

        var consumerHarness = harness.GetConsumerHarness<CryptoVisitedConsumer>();
        var eventConsumed = await consumerHarness.Consumed.Any<Visited>();
        eventConsumed.Should().BeTrue();
    }
}