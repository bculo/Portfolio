using Microsoft.Extensions.DependencyInjection;

namespace Trend.IntegrationTests
{
    [Collection("TrendCollection")]
    public abstract class TrendControllerBaseTest : IAsyncLifetime
    {
        private readonly IServiceScope _scope;
        protected readonly HttpClient Client;
        protected readonly TrendApiFactory Factory;
        protected readonly TrendFixtureService FixtureService;

        protected TrendControllerBaseTest(TrendApiFactory factory)
        {
            Factory = factory;
            Client = factory.Client;
            _scope = Factory.Services.CreateScope();
            FixtureService = _scope.ServiceProvider.GetRequiredService<TrendFixtureService>();
        }

        public virtual async Task DisposeAsync()
        {
            await FixtureService.DropDatabase();
            _scope.Dispose();
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
