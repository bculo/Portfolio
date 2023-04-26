using Crypto.IntegrationTests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.Common
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IApiFactory _factory;
        protected readonly HttpClient _client;

        public BaseTests(IApiFactory factory)
        {
            _factory = factory;
            _client = factory.Client;
        }

        public virtual async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;
    }
}
