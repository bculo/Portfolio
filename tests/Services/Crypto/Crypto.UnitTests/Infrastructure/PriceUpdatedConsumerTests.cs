using System.Runtime.Intrinsics.X86;
using Crypto.Infrastructure.Consumers;
using Events.Common.Crypto;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.UnitTests.Infrastructure;

public class PriceUpdatedConsumerTests : IClassFixture<MassTransitFixture>
{
    private readonly MassTransitFixture _fixture;
    
    public PriceUpdatedConsumerTests(MassTransitFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ShouldReceiveEvent_WhenPublished()
    {
        var harness = _fixture.GetTestHarness();
        await harness.Start();
        
        await harness.Bus.Publish(new PriceUpdated
        {
            Price = 12m,
            Id = Guid.NewGuid(),
            Name = "Bitcoin",
            Symbol = "BTC"
        });

        var consumerHarness = harness.GetConsumerHarness<PriceUpdatedConsumer>();
        var eventConsumed = await consumerHarness.Consumed.Any<PriceUpdated>();
        eventConsumed.Should().BeTrue();
    }
}