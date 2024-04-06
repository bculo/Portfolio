using AutoMapper;
using Crypto.Infrastructure.Consumers;
using Crypto.Infrastructure.Consumers.State;
using MassTransit;
using MassTransit.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.UnitTests.Infrastructure;

public class MassTransitFixture : IDisposable
{
    public IServiceScope Scope { get; } 

    public MassTransitFixture()
    {
        Scope = new ServiceCollection()
            .AddScoped<IMediator>(sc => Substitute.For<IMediator>())
            .AddScoped<IFusionCache>(sc => Substitute.For<IFusionCache>())
            .AddScoped<IMapper>(sc => Substitute.For<IMapper>())
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<PriceUpdatedConsumer>();
                x.AddConsumer<AddCryptoItemConsumer>();
                x.AddConsumer<CryptoVisitedConsumer>();
                x.AddConsumer<UpdateCryptoItemsPriceConsumer>();

                x.AddSagaStateMachine<AddCryptoItemStateMachine, AddCryptoItemState>();
            })
            .BuildServiceProvider(true)
            .CreateScope();
    }

    public ITestHarness GetTestHarness()
    {
        return Scope.ServiceProvider.GetRequiredService<ITestHarness>();
    }
    
    public void Dispose()
    {
        Scope.Dispose();
    }
}