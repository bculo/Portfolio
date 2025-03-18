using System.Security.Claims;
using AutoFixture;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using Tests.Common.Jwt;

namespace Crypto.IntegrationTests;

[Collection(nameof(CryptoApiCollection))]
public abstract class BaseCryptoEndpointTests(CryptoApiFactory factory) : IAsyncLifetime
{
    protected HttpClient Client { get; } = factory.Client;
    protected CryptoApiFactory Factory { get; } = factory;

    protected Fixture MockFixture { get;  } = new Fixture().Configure();

    protected TestFixture Fixture { get; private set; } = null!;
    protected ITestHarness MessageQueue { get; private set; } = null!;

    
    private ITokenGenerator _tokenGenerator = null!;

    private IServiceScope _scope = null!;

    public virtual Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }

    public virtual Task InitializeAsync()
    {
        _scope = Factory.Services.CreateAsyncScope();
        
        Fixture = _scope.ServiceProvider.GetRequiredService<TestFixture>();
        _tokenGenerator  = _scope.ServiceProvider.GetRequiredService<ITokenGenerator>();
        MessageQueue = _scope.ServiceProvider.GetTestHarness();
        
        return Task.CompletedTask;
    }

    protected async Task Authenticate(UserRole role)
    {
        var token = await _tokenGenerator.GenerateToken([new Claim(ClaimTypes.Role, role.ToString())]);
        Client.AddJwtToken(token);
    }
}

