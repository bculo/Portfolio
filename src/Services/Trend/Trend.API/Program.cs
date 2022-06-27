using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;
using Trend.API.Filters;
using Trend.API.Filters.Action;
using Trend.Application;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
})
.AddFluentValidation();

builder.Services.AddLocalization();


builder.Services.AddScoped<CacheActionFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApplicationLayer.AddServices(builder.Configuration, builder.Services);
//ApplicationLayer.AddBackgroundServies(builder.Configuration, builder.Services);
ApplicationLayer.AddLogger(builder);

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
