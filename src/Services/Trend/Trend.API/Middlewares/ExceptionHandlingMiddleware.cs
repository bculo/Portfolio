using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace Trend.API.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = "An server error occurred"
            };
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}