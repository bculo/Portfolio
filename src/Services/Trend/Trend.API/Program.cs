using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Trend.API.Extensions;
using Trend.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApiProject();
await builder.SeedAll();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(app.Configuration["KeycloakOptions:ApplicationName"]);
        options.OAuthRealm(app.Configuration["KeycloakOptions:RealmName"]);

        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
}

app.UseAuthentication();

app.UseAuthorization();

app.UseRequestLocalization();

app.UseSerilogRequestLogging();

app.UseHangfireDashboard();

app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseOutputCache();

app.MapControllers();

app.Run();

//For testing purpose
public partial class Program { }