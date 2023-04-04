using Crypto.API.Filters;
using Crypto.Application;
using Crypto.Application.Interfaces.Services;
using Crypto.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApplicationLayer.AddServices(builder.Services, builder.Configuration);

InfrastractureLayer.ConfigureMessageQueue(builder.Services, builder.Configuration, true);
InfrastractureLayer.AddServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

//For testing purpose
public partial class Program { }
