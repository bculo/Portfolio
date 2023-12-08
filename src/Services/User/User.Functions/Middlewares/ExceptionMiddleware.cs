using MassTransit.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User.Application.Common.Exceptions;
using User.Application.Entities;
using User.Functions.Extensions;

namespace User.Functions.Middlewares
{
    public class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var requestData = await context.GetHttpRequestDataAsync();

                //for some reason all thrown exceptions are wrapped by the AggregateException in azure functions
                var realException = ex.InnerException ?? ex;

                if(realException is PortfolioUserValidationException) 
                { 
                    var validationException = realException as PortfolioUserValidationException;
                    await requestData.DefineResponseMiddleware(HttpStatusCode.BadRequest, validationException.Errors, true);
                    return;
                }

                if(realException is PortfolioUserNotFoundException)
                {
                    var notFoundException = realException as PortfolioUserNotFoundException;
                    await requestData.DefineResponseMiddleware(HttpStatusCode.NotFound, notFoundException.UserMessage);
                    return;
                }

                if (realException is PortfolioUserCoreException)
                {
                    var coreException = realException as PortfolioUserCoreException;
                    await requestData.DefineResponseMiddleware(HttpStatusCode.BadRequest, coreException.UserMessage);
                    return;
                }

                await requestData.DefineResponseMiddleware(HttpStatusCode.InternalServerError, "Error occurred.");
            }
        }
    }
}
