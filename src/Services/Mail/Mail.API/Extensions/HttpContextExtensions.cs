using System.Net;
using Mail.Application.Exceptions;
using Mail.Application.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Mail.API.Extensions;

public static class HttpContextExtensions
{
    public static async Task HandleResponse(this HttpContext context)
    {
        var error = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        if (error is null || error is not MailCoreException)
        {
            return;
        }
        
        switch (error)
        {
            case MailCoreNotFoundException:
                await SetResponseBody(context, HttpStatusCode.NotFound, new ExceptionResponseDto
                {
                    Message = error.Message
                });
                break;
            case MailValidationException exception:
            {
                var validationException = exception;
                await SetResponseBody(context, HttpStatusCode.NotFound, new ExceptionValidationResponseDto
                {
                    Message = validationException.Message,
                    Errors = validationException.Errors
                });
                break;
            }
            default:
                await SetResponseBody(context, HttpStatusCode.BadRequest, new ExceptionResponseDto
                {
                    Message = error.Message
                });
                break;
        }
    }

    private static async Task SetResponseBody(HttpContext context, 
        HttpStatusCode code,
        ExceptionResponseDto responseContent)
    {
        context.Response.StatusCode = (int)code;
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await context.Response.WriteAsJsonAsync(responseContent);
    }
}