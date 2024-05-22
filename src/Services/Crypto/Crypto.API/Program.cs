using Crypto.API.Configurations;
using Crypto.API.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((host, log) =>
{
    log.MinimumLevel.Information();
    log.WriteTo.Console(theme: AnsiConsoleTheme.Code);
});

builder.Services.ConfigureApiProject(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(app.Configuration["KeycloakOptions:ApplicationName"]);
        options.OAuthRealm(app.Configuration["KeycloakOptions:RealmName"]);
    });

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();

//For testing purpose
public partial class Program { }
