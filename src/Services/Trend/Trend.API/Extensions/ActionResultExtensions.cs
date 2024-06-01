using System.Net;
using System.Text.Json;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trend.Domain.Errors;
using WebProject.Common.Extensions;

namespace Trend.API.Extensions;

public static class ActionResultExtensions
{
    public static IActionResult ToProblemDetails(this CoreError errors)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = errors.Description,
            Type = errors.Code,
        };
        
        if (errors is NotFoundError)
        {
            problemDetails.Title = "Item not found";
            problemDetails.Status = StatusCodes.Status404NotFound;
            return new NotFoundObjectResult(problemDetails);
        }

        if (errors is ValidationError validationError)
        {
            return new BadRequestObjectResult(validationError.Errors.ToValidationProblemDetails());
        }
        
        problemDetails.Title = "Bad request";
        problemDetails.Status = StatusCodes.Status400BadRequest;
        return new BadRequestObjectResult(problemDetails);
    }
    
    public static IActionResult ToActionResult<T>(this Either<CoreError, T> t)
    {
        return t.Match(x =>
        {
            if (x is Unit)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(x);
        }, error => error.ToProblemDetails());
    }
}