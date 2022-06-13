using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trend.API.Filters.Models;
using Trend.Domain.Exceptions;

namespace Trend.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            if(context.Exception is TrendAppCoreException)
            {
                HandleCustomException(context);
                return;
            }

            var errorResponse = new ErrorResponseModel
            {
                Message = "Unknown exception",
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }

        private void HandleCustomException(ExceptionContext context)
        {
            var trendException = context.Exception as TrendAppCoreException;

            if(trendException is TrendNotFoundException)
            {
                context.Result = new NotFoundObjectResult(trendException.UserMessage);
                context.ExceptionHandled = true;
                return;
            }

            var errorResponse = new ErrorResponseModel
            {
                Message = context.Exception.Message,
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.Result = new BadRequestObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
