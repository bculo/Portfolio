using FluentValidation.AspNetCore;
using Serilog;
using Trend.API.Filters;
using Trend.API.Filters.Action;
using Trend.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
}).AddFluentValidation();

builder.Services.AddScoped<CacheActionFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApplicationLayer.AddServices(builder.Configuration, builder.Services);
//ApplicationLayer.AddBackgroundServies(builder.Configuration, builder.Services);
ApplicationLayer.AddLogger(builder);

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

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
