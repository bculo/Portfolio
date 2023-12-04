using Hangfire;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Trend.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApiProject();

var app = builder.Build();

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

app.UseRequestLocalization();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.UseOutputCache();

app.Run();

//For testing purpose
public partial class Program { }