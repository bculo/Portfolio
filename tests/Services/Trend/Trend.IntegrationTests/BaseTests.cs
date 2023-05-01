using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.IntegrationTests.Helpers;

namespace Trend.IntegrationTests
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IApiFactory _factory;
        private readonly HttpClient _client;

        public BaseTests(IApiFactory factory)
        {
            _factory = factory;
            _client = _factory.Client;
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected HttpClient GetAuthInstance(UserAuthType type)
        {
            return type switch
            {
                UserAuthType.None => _client.RemoveJwtToken(),
                UserAuthType.User => _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN),
                _ => throw new NotSupportedException("Auth type not supported")
            };
        }
    }

    public enum UserAuthType
    {
        None = 0,
        User = 1
    }
}
