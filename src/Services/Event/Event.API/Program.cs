using Event.API.Constants;
using Event.API.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CryptoPriceUpdatedConsumer>().Endpoint(config =>
    {
        config.Name = $"{RabbitMqConstants.RABBITMQ_CONSUMER_PREFIX}-cryptopriceupdated";
    });

    x.AddConsumer<NewCryptoAddedConsumer>().Endpoint(config =>
    {
        config.Name = $"{RabbitMqConstants.RABBITMQ_CONSUMER_PREFIX}-newcryptoadded";
    });

    x.AddConsumer<CryptoInfoUpdatedConsumer>().Endpoint(config =>
    {
        config.Name = $"{RabbitMqConstants.RABBITMQ_CONSUMER_PREFIX}-cryptoinfoupdated";
    });

    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["QueueOptions:Address"]);
        config.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
