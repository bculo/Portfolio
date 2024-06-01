using Microsoft.Extensions.DependencyInjection;

namespace Trend.IntegrationTests
{
    [Collection("TrendCollection")]
    public abstract class TrendControllerBaseTest(TrendApiFactory factory) : IAsyncLifetime
    {
        protected readonly HttpClient Client = factory.Client;
        protected readonly TrendApiFactory Factory = factory;
        
        private IServiceScope _scope = default!;
        protected TrendFixtureService FixtureService { get; private set; } = default!;
        
        public virtual async Task DisposeAsync()
        {
            await FixtureService.DropDatabase();
            _scope.Dispose();
        }

        public virtual Task InitializeAsync()
        {
            _scope = Factory.Services.CreateAsyncScope();
            FixtureService = _scope.ServiceProvider.GetRequiredService<TrendFixtureService>();
            return Task.CompletedTask;
        }
    }
}
