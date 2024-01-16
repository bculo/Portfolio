using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using OpenTelemetry.Trace;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;
using Trend.Domain.Exceptions;

namespace Trend.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly ITransaction _session;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, ITransaction session)
        {
            _logger = logger;
            _session = session;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            Activity.Current?.RecordException(context.Exception);

            HandleSession();

            if(context.Exception is TrendAppCoreException)
            {
                HandleCustomException(context);
                return;
            }
            
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            context.ExceptionHandled = true;
        }

        private void HandleSession()
        { 
            _session.AbortTransaction();
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
