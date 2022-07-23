using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Trend.Application.Interfaces;

namespace Trend.API.Filters.Action
{
    public class CacheActionFilter : IAsyncActionFilter
    {
        private readonly ICacheService _cache;
        private readonly ILogger<CacheActionFilter> _logger;

        public CacheActionFilter(ICacheService cache, ILogger<CacheActionFilter> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogTrace("Method {0} called in service {1}", 
                nameof(OnActionExecutionAsync), 
                nameof(CacheActionFilter));

            var actionIdentifier = GetIdentifier(context);
            var cahceResult = await _cache.Get(actionIdentifier);

            if(cahceResult != null)
            {
                _logger.LogTrace("Item founded in cache for action {0}", actionIdentifier);

                context.Result = new OkObjectResult(cahceResult);
                return;
            }

            var action = await next();

            if(action?.Result is OkObjectResult)
            {
                _logger.LogTrace("Action returnd OkObjectResult. Caching resultset");

                var actionResult = action.Result as OkObjectResult;
                await _cache.Add(GetIdentifier(context), actionResult!.Value);
            }
        }

        private string GetIdentifier(ActionExecutingContext context)
        {
            _logger.LogTrace("Method {0} called", nameof(GetIdentifier));

            if(context.ActionDescriptor is null)
            {
                return "EMPTY";
            }

            var descriptor = context.ActionDescriptor;

            return $"{descriptor.DisplayName}";
        }
    }
}
