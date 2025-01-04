using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.Common
{
    [Collection(nameof(CryptoApiCollection))]
    public abstract class BaseCryptoEndpointTests(CryptoApiFactory factory) : IAsyncLifetime
    {
        protected HttpClient Client { get; } = factory.Client;
        protected CryptoApiFactory Factory { get; } = factory;

        protected Fixture MockFixture { get;  } = new Fixture().Configure();

        protected DataFixture DataManager { get; private set; } = default!;
        

        private IServiceScope _scope = default!;

        public virtual Task DisposeAsync()
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }

        public virtual Task InitializeAsync()
        {
            _scope = Factory.Services.CreateAsyncScope();
            DataManager = _scope.ServiceProvider.GetRequiredService<DataFixture>();
            return Task.CompletedTask;
        }
    }
}
