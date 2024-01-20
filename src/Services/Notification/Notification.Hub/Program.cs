using Notification.Hub;
using Notification.Hub.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSignalRHubApp();

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

