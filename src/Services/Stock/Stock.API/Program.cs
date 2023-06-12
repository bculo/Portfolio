using Hangfire;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;
using Stock.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((host, log) =>
{
    log.WriteTo.Console();
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

        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });

    app.UseHangfireDashboard();
    app.UseCors();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
