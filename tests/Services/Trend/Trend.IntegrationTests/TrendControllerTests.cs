using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Extensions;

namespace Trend.IntegrationTests
{
    [Collection("TrendCollection")]
    public abstract class TrendControllerTests : IAsyncLifetime
    {
        private readonly IServiceScope _scope;
        protected readonly TrendApiFactory _factory;
        protected readonly TrendFixtureService _fixtureService;

        protected TrendControllerTests(TrendApiFactory factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _fixtureService = _scope.ServiceProvider.GetRequiredService<TrendFixtureService>();
        }

        public virtual async Task DisposeAsync()
        {
            await _fixtureService.DropDatabase();
            _scope.Dispose();
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
