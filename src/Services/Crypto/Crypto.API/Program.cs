using Crypto.API.Filters;
using Crypto.API.SignalR;
using Crypto.Application;
using Crypto.Application.Interfaces.Services;
using Crypto.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<INotificationService, NotificationService>();

builder.Services.AddSignalR();

ApplicationLayer.AddServices(builder.Services, builder.Configuration);
ApplicationLayer.ConfigureMessageQueue(builder.Services, builder.Configuration, true);
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

app.MapHub<CryptoHub>("/cryptohub");

app.Run();

//For testing purpose
public partial class Program { }
