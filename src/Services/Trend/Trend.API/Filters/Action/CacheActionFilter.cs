using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Trend.Application.Interfaces;

namespace Trend.API.Filters.Action
{
    public class CacheActionFilter : IAsyncActionFilter
    {
        private readonly ICacheService _cache;

        public CacheActionFilter(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cahceResult = await _cache.Get(GetIdentifier(context));

            if(cahceResult != null)
            {
                var temp = JsonConvert.DeserializeObject(cahceResult);
                var cacheResponseFormatted = JsonConvert.SerializeObject(temp, Formatting.Indented);
                context.Result = new OkObjectResult(cacheResponseFormatted);
                return;
            }

            var action = await next();

            if(action?.Result is OkObjectResult)
            {
                var actionResult = action.Result as OkObjectResult;
                await _cache.Add(GetIdentifier(context), actionResult?.Value);
            }
        }

        private string GetIdentifier(ActionExecutingContext context)
        {
            if(context.ActionDescriptor is null)
            {
                return "EMPTY";
            }

            var descriptor = context.ActionDescriptor;

            return $"{descriptor.DisplayName}";
        }

        private bool IsArray(string value)
        {
            return value.TrimStart().StartsWith("[") && value.TrimEnd().EndsWith("]");
        }
    }
}
