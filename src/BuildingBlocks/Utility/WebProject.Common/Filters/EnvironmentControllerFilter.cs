﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebProject.Common.Filters
{
    public class EnvironmentControllerFilter : TypeFilterAttribute
    {
        public EnvironmentControllerFilter(params string[] environments) : base(typeof(EnvironmentActionFilterImplementation))
        {
            Arguments = new object[] { environments };
        }

        private class EnvironmentActionFilterImplementation : IAsyncActionFilter
        {
            private readonly ILogger<EnvironmentControllerFilter> _logger;
            private readonly WebApplication _environment;

            private readonly IEnumerable<string> _allowedEnvironments;

            public EnvironmentActionFilterImplementation(ILogger<EnvironmentControllerFilter> logger,
                WebApplication environment,
                string[] allowedEnvironments)
            {
                _logger = logger;
                _environment = environment;

                _allowedEnvironments = allowedEnvironments != null ? allowedEnvironments.ToList() : Enumerable.Empty<string>();
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!_allowedEnvironments.Any())
                {
                    await next();
                    return;
                }

                if (_allowedEnvironments.Contains(_environment.Environment.EnvironmentName))
                {
                    await next();
                    return;
                }
                
                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Current environment not supported"
                });
            }
        }
    }
}
