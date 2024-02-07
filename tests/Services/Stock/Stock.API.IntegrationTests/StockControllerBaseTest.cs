using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stock.Infrastructure.Persistence;

namespace Stock.API.IntegrationTests;

[Collection("StockCollection")]
public class StockControllerBaseTest : IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly StockApiFactory Factory;

    protected StockSeedHelper Helper { get; private set; } = default!;
        
    private IServiceScope _scope = default!;
    
    protected StockControllerBaseTest(StockApiFactory factory)
    {
        Factory = factory;
        Client = factory.Client;
    }

    public virtual async Task DisposeAsync()
    {
        Helper.Dispose();
        _scope.Dispose();

        await Factory.ResetDatabaseAsync();
    }

    public virtual Task InitializeAsync()
    {
        _scope = Factory.Services.CreateAsyncScope();
        Helper = _scope.ServiceProvider.GetRequiredService<StockSeedHelper>();
        return Task.CompletedTask;
    }
}