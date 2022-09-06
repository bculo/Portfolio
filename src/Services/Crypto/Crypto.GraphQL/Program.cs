using Crypto.Application;
using Crypto.GraphQL;
using Crypto.GraphQL.Configuration;
using Crypto.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<FetchAllResponseDtoType>();


builder.Services.AddControllers();

ApplicationLayer.AddServices(builder.Services, builder.Configuration);
ApplicationLayer.ConfigureRabbitMQ(builder.Services, builder.Configuration);
InfrastractureLayer.AddServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();
