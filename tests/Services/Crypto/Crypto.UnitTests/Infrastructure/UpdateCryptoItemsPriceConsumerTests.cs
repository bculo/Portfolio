using Crypto.Infrastructure.Consumers;
using Events.Common.Crypto;
using FluentAssertions;

namespace Crypto.UnitTests.Infrastructure;

public class UpdateCryptoItemsPriceConsumerTests : IClassFixture<MassTransitFixture>
{
    private readonly MassTransitFixture _fixture;
    
    public UpdateCryptoItemsPriceConsumerTests(MassTransitFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task ShouldReceiveEvent_WhenPublished()
    {
        var harness = _fixture.GetTestHarness();
        await harness.Start();
        
        await harness.Bus.Publish(new UpdateItemsPrices
        {
             Time = DateTimeOffset.UtcNow
        });

        var consumerHarness = harness.GetConsumerHarness<UpdateCryptoItemsPriceConsumer>();
        var eventConsumed = await consumerHarness.Consumed.Any<UpdateItemsPrices>();
        eventConsumed.Should().BeTrue();
    }
}