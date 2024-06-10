using Tracker.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiProject(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(app.Configuration["KeycloakOptions:ApplicationName"]);
        options.OAuthRealm(app.Configuration["KeycloakOptions:RealmName"]);
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
