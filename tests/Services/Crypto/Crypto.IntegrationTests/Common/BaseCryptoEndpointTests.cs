using System.Security.Claims;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using Tests.Common.Jwt;

namespace Crypto.IntegrationTests.Common
{
    [Collection(nameof(CryptoApiCollection))]
    public abstract class BaseCryptoEndpointTests(CryptoApiFactory factory) : IAsyncLifetime
    {
        protected HttpClient Client { get; } = factory.Client;
        protected CryptoApiFactory Factory { get; } = factory;

        protected Fixture MockFixture { get;  } = new Fixture().Configure();

        protected TestFixture Fixture { get; private set; } = default!;
        protected ITokenGenerator TokenGenerator { get; private set; } = default!;
        

        private IServiceScope _scope = default!;

        public virtual Task DisposeAsync()
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }

        public virtual Task InitializeAsync()
        {
            _scope = Factory.Services.CreateAsyncScope();
            
            Fixture = _scope.ServiceProvider.GetRequiredService<TestFixture>();
            TokenGenerator  = _scope.ServiceProvider.GetRequiredService<ITokenGenerator>();
            
            return Task.CompletedTask;
        }

        public async Task Authenticate(UserRole role)
        {
            var token = await TokenGenerator.GenerateToken([new Claim(ClaimTypes.Role, role.ToString())]);
            Client.AddJwtToken(token);
        }
    }
}
