using Notification.Application;
using Notification.Application.Interfaces;
using Notification.Hub;
using Notification.Hub.Configurations;
using Notification.Hub.Extensions;
using Notification.Hub.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INotificationService, SignalRNotificationService>();

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureSignalR(builder.Configuration);

ApplicationLayer.ConfigureMessageQueue(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("CorsPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PortfolioHub>("/portfolio");

app.Run();

