﻿using Crypto.API.Filters.Models;
using Crypto.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crypto.API.Filters
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

            if (context.Exception is CryptoCoreException)
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
            var trendException = context.Exception as CryptoCoreException;

            if (trendException is CryptoCoreNotFoundException)
            {
                context.Result = new NotFoundObjectResult(trendException.UserMessage);
                context.ExceptionHandled = true;
                return;
            }

            if(trendException is CryptoCoreValidationException)
            {
                var validationException = context.Exception as CryptoCoreValidationException;

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
