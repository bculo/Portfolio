using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProject.Common.Extensions;

public static class ProblemDetailsExtensions
{
    public static HttpValidationProblemDetails ToValidationProblemDetails(this IDictionary<string, string[]> errors,
        string? type = default)
    {
        var validationProblem = new HttpValidationProblemDetails(errors)
        {
            Type = type
        };
        return validationProblem;
    }
}