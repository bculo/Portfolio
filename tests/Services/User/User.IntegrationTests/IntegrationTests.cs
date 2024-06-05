using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace User.IntegrationTests;

[CollectionDefinition(nameof(UserCollection))]
public class UserCollection : ICollectionFixture<UserFactory>
{
    
}

[Collection(nameof(UserCollection))]
public abstract class IntegrationTests(UserFactory factory) : IAsyncLifetime
{
    protected readonly UserFactory Factory = factory;
    protected UserTestFixture Fixture { get; private set; } = default!;
    
    public Task InitializeAsync()
    {
        var scope = Factory.Services.CreateAsyncScope();
        Fixture = scope.ServiceProvider.GetRequiredService<UserTestFixture>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}