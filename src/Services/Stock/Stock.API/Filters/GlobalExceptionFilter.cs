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
            if (context.Exception is StockCoreException stockException)
            {
                HandleCustomException(context, stockException);
                return;
            }
            
            var errorResponse = new ErrorResponseModel
            {
                Message = "Unknown exception",
                StatusCode = StatusCodes.Status400BadRequest
            };

            _logger.LogError("Exception occurred: {Exception}", context.Exception);
            
            context.Result = new BadRequestObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }

        private void HandleCustomException(ExceptionContext context, StockCoreException exception)
        {
            _logger.LogError("Custom exception with code {Code} occurred: {Exception} ", exception.Code, exception);
            
            if (exception is StockCoreNotFoundException notFoundException)
            {
                context.Result = new NotFoundObjectResult(notFoundException.Message);
                context.ExceptionHandled = true;
                return;
            }

            if (exception is StockCoreValidationException)
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
