using Crypto.Infrastructure.Consumers;
using Events.Common.Crypto;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.UnitTests.Infrastructure;

public class AddCryptoItemConsumerTests(MassTransitFixture fixture) : IClassFixture<MassTransitFixture>
{
    [Fact]
    public async Task ShouldReceiveEvent_WhenPublished()
    {
        var harness = fixture.GetTestHarness();
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