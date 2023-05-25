using Crypto.IntegrationTests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.IntegrationTests.Constants;
using Microsoft.IdentityModel.JsonWebTokens;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.Common
{
    [Collection("CryptoCollection")]
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly CryptoApiFactory _factory;
        protected readonly HttpClient _client;

        public BaseTests(CryptoApiFactory factory)
        {
            _factory = factory;
            _client = factory.Client;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual async Task InitializeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        public HttpClient GetAuthInstance()
        {
            return _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
        }
    }
}
