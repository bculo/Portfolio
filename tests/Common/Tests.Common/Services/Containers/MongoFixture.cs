using DotNet.Testcontainers.Containers;
using Testcontainers.MongoDb;

namespace Tests.Common.Services.Containers;

public class MongoFixture : ContainerFixture
{
    private readonly MongoDbContainer _container;

    public MongoFixture()
    {
        _container = new MongoDbBuilder()
            .WithImage("mongo:7.0.4")
            .WithName($"Mongo.{Guid.NewGuid()}")
            .Build();
    }

    public MongoFixture(MongoCredentials credentials)
    {
        _container = new MongoDbBuilder()
            .WithImage("mongo:7.0.4")
            .WithUsername(credentials.UserName)
            .WithPassword(credentials.Password)
            .WithName($"Mongo.{Guid.NewGuid()}")
            .Build();
    }

    protected override DockerContainer Container => _container;

    public override string GetConnectionString()
    {
        return _container.GetConnectionString();
    }
}

public record MongoCredentials(string UserName, string Password)
{
    public string UserName { get; private set; } = UserName ?? throw new ArgumentNullException(nameof(UserName));
    public string Password { get; private set; } = Password ?? throw new ArgumentNullException(nameof(Password));
}