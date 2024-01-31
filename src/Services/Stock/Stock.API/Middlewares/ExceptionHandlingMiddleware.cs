using Microsoft.AspNetCore.Mvc;
using Stock.Core.Exceptions;

namespace Stock.API.Middlewares;

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
            if (exception is StockCoreException coreException)
            {
                HandleCoreException(context, coreException);
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
    
    private async Task HandleCoreException(HttpContext context, StockCoreException exception)
    {
        _logger.LogError("Custom exception with code {Code} occurred: {Exception} ", exception.Code, exception);
            
        var problem = new ProblemDetails
        {
            Detail = exception.Message,
            Type = exception.Code
        };
        
        if (exception is StockCoreNotFoundException notFoundException)
        {
            problem.Title = "Item not found";
            problem.Status = StatusCodes.Status404NotFound;
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(problem);
            return;
        }

        if (exception is StockCoreValidationException validationException)
        {
            problem.Title = "Validation exception";
            problem.Extensions = new Dictionary<string, object?>();
            problem.Extensions.Add("errors", validationException.Errors);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem);
        }

        problem.Status = StatusCodes.Status400BadRequest;
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(problem);
    }
}