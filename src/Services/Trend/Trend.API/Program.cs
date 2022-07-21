using FluentValidation.AspNetCore;
using Keycloak.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using Trend.API.Extensions;
using Trend.API.Filters;
using Trend.API.Filters.Action;
using Trend.Application;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
})
.AddFluentValidation();

builder.Services.ConfigureAuthorization(builder.Configuration);

builder.Services.AddLocalization();

builder.Services.AddScoped<CacheActionFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http, 
            Scheme = JwtBearerDefaults.AuthenticationScheme
        }
    );

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                    Type = ReferenceType.SecurityScheme
                }
            },new List<string>()
        }
    });
});

ApplicationLayer.AddServices(builder.Configuration, builder.Services);

ApplicationLayer.AddLogger(builder.Host);

builder.Services.Configure<RequestLocalizationOptions>(opts =>
{
    var hrCulture = new CultureInfo("hr");
    var enCulture = new CultureInfo("en");
    var supportedCultures = new[]
    {
        hrCulture,
        enCulture
    };
    opts.DefaultRequestCulture = new RequestCulture(enCulture, enCulture);
    opts.SupportedCultures = supportedCultures;
    opts.SupportedUICultures = supportedCultures;
});


builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());

app.UseRequestLocalization();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
