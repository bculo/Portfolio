using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Extensions;

namespace Trend.IntegrationTests
{
    [Collection("TrendCollection")]
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly TrendApiFactory _factory;

        public BaseTests(TrendApiFactory factory)
        {
            _factory = factory;
        }

        public virtual async Task DisposeAsync()
        {
            await _factory.ResetDatabaseState();
            await _factory.SeedDatabase();
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected HttpClient GetAuthInstance(UserAuthType type)
        {
            return type switch
            {
                UserAuthType.None => _factory.Client.RemoveHeaderValue("UserAuthType"),
                UserAuthType.User => _factory.Client.AddHeaderValue("UserAuthType", ((int)UserAuthType.User).ToString()),
                _ => _factory.Client
            };
        }
    }

    public enum UserAuthType
    {
        None = 0,
        User = 1
    }
}
