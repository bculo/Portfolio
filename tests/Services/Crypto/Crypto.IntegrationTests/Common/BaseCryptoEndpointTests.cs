using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.Common
{
    [Collection(nameof(CryptoApiCollection))]
    public abstract class BaseCryptoEndpointTests : IAsyncLifetime
    {
        public HttpClient Client { get; }
        public CryptoApiFactory Factory { get; }
        
        public Fixture MockFixture { get;  }

        public DataFixture DataManager { get; private set; } = default!;
        

        private IServiceScope _scope = default!;

        protected BaseCryptoEndpointTests(CryptoApiFactory factory)
        {
            Factory = factory;
            Client = factory.Client;
            MockFixture = new Fixture().Configure();
        }

        public virtual async Task DisposeAsync()
        {
            _scope.Dispose();

            // await Factory.ResetDatabaseAsync();
        }

        public virtual Task InitializeAsync()
        {
            _scope = Factory.Services.CreateAsyncScope();
            
            DataManager = _scope.ServiceProvider.GetRequiredService<DataFixture>();
            
            return Task.CompletedTask;
        }
    }
}
