using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using Trend.API.Filters.Models;
using Trend.Domain.Exceptions;

namespace Trend.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IClientSessionHandle _session;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IClientSessionHandle session)
        {
            _logger = logger;
            _session = session;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            HandleSession();

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

        private void HandleSession()
        {
            try
            {
                if(_session.IsInTransaction)
                {
                    _session.AbortTransaction();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message, e);
            }
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
