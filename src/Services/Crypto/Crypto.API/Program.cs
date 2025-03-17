using Crypto.API.Configurations;
using Hangfire;
using Keycloak.Common.Middlewares;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, log) =>
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
        options.OAuthClientId(app.Configuration["AuthOptions:ApplicationName"]);
        options.OAuthRealm(app.Configuration["AuthOptions:RealmName"]);
        options.OAuthScopes(app.Configuration["AuthOptions:Scopes"]);
    });

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
}

app.UseExceptionHandler();

app.UseAccessTokenContextMiddleware();

app.UseAuthentication();

app.UseCallerRoleMapperMiddleware();

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();

//For testing purpose
public partial class Program { }
