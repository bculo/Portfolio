using Carter;
using Mail.API.Extensions;
using Mail.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureMinimalApiProject(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var error = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        if (error is not null && error is MailCoreException)
        {
            var response = new { message = error.Message };
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.Run();