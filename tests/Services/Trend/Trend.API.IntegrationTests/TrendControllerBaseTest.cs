using Microsoft.Extensions.DependencyInjection;

namespace Trend.IntegrationTests
{
    [Collection("TrendCollection")]
    public abstract class TrendControllerBaseTest : IAsyncLifetime
    {
        protected readonly HttpClient Client;
        protected readonly TrendApiFactory Factory;
        
        private IServiceScope _scope = default!;
        protected TrendFixtureService FixtureService { get; private set; } = default!;
        

        protected TrendControllerBaseTest(TrendApiFactory factory)
        {
            Factory = factory;
            Client = factory.Client;
        }

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
