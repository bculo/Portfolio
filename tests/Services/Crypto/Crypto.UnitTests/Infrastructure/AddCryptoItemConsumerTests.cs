using Crypto.Infrastructure.Consumers;
using Events.Common.Crypto;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.UnitTests.Infrastructure;

public class AddCryptoItemConsumerTests : IClassFixture<MassTransitFixture>
{
    private readonly MassTransitFixture _fixture;
    
    public AddCryptoItemConsumerTests(MassTransitFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ShouldReceiveEvent_WhenPublished()
    {
        var harness = _fixture.GetTestHarness();
        await harness.Start();
        
        await harness.Bus.Publish(new AddItem
        {
            CorrelationId = Guid.NewGuid(),
            Symbol = "BTC"
        });

        var consumerHarness = harness.GetConsumerHarness<AddCryptoItemConsumer>();
        var eventConsumed = await consumerHarness.Consumed.Any<AddItem>();
        eventConsumed.Should().BeTrue();
    }
}