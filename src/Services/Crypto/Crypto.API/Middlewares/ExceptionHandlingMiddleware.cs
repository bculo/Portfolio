using Crypto.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            if (exception is CryptoCoreException coreException)
            {
                await HandleCoreException(context, coreException);
                return;
            }
            
            _logger.LogError("Exception occurred: {Exception}", exception);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error"
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
    
    private async Task HandleCoreException(HttpContext context, CryptoCoreException exception)
    {
        _logger.LogError("Custom exception occurred: {Exception} ", exception);
            
        var problem = new ProblemDetails
        {
            Detail = exception.Message,
        };
        
        if (exception is CryptoCoreNotFoundException notFoundException)
        {
            problem.Title = "Item not found";
            problem.Status = StatusCodes.Status404NotFound;
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problem);
            return;
        }

        if (exception is CryptoCoreValidationException validationException)
        {
            problem.Title = "Validation exception";
            problem.Extensions = new Dictionary<string, object?>();
            problem.Extensions.Add("errors", validationException.Errors);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem);
            return;
        }

        problem.Status = StatusCodes.Status400BadRequest;
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(problem);
    }
}