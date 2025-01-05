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
    private readonly IServiceScope _scope;

    public MassTransitFixture()
    {
        _scope = new ServiceCollection()
            .AddScoped<IMediator>(_ => Substitute.For<IMediator>())
            .AddScoped<IFusionCache>(_ => Substitute.For<IFusionCache>())
            .AddScoped<IMapper>(_ => Substitute.For<IMapper>())
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
        return _scope.ServiceProvider.GetRequiredService<ITestHarness>();
    }
    
    public void Dispose()
    {
        _scope.Dispose();
    }
}