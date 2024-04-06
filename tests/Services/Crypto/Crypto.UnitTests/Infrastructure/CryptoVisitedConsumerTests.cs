using Crypto.Infrastructure.Consumers;
using Events.Common.Crypto;
using FluentAssertions;

namespace Crypto.UnitTests.Infrastructure;

public class CryptoVisitedConsumerTests : IClassFixture<MassTransitFixture>
{
    private readonly MassTransitFixture _fixture;
    
    public CryptoVisitedConsumerTests(MassTransitFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task ShouldReceiveEvent_WhenPublished()
    {
        var harness = _fixture.GetTestHarness();
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