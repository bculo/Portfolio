using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.IntegrationTests.Common
{
    [Collection(nameof(CryptoApiCollection))]
    public abstract class BaseCryptoEndpointTests : IAsyncLifetime
    {
        public HttpClient Client { get; }
        public CryptoApiFactory Factory { get; }
        
        public Fixture MockFixture { get;  }
        

        private IServiceScope _scope = default!;

        protected BaseCryptoEndpointTests(CryptoApiFactory factory)
        {
            Factory = factory;
            Client = factory.Client;
            MockFixture = new Fixture();
        }

        public virtual async Task DisposeAsync()
        {
            _scope.Dispose();

            await Factory.ResetDatabaseAsync();
        }

        public virtual Task InitializeAsync()
        {
            _scope = Factory.Services.CreateAsyncScope();
            return Task.CompletedTask;
        }
    }
}
