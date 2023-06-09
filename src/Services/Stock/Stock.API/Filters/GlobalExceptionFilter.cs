using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Stock.Core.Exceptions;
using Stock.API.Filters.Models;

namespace Stock.API.Filters
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

            if (context.Exception is StockCoreException)
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
            var trendException = context.Exception as StockCoreException;

            if (trendException is StockCoreNotFoundException)
            {
                context.Result = new NotFoundObjectResult(trendException.UserMessage);
                context.ExceptionHandled = true;
                return;
            }

            if (trendException is StockCoreValidationException)
            {
                var validationException = context.Exception as StockCoreValidationException;

                var validatioResponse = new ValidationErrorResponse
                {
                    Errors = validationException!.Errors,
                    Message = validationException.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(validatioResponse);
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
