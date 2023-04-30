using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IApiFactory _factory;
        protected readonly HttpClient Client;

        public BaseTests(IApiFactory factory)
        {
            _factory = factory;
            Client = _factory.Client;
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
