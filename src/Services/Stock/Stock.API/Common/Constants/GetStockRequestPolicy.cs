using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;
using Stock.Application.Common.Configurations;

namespace Stock.API.Common.Constants;

public class GetStockRequestPolicy : IOutputCachePolicy
{
    ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var attemptOutputCaching = AttemptOutputCaching(context);
        
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;
        
        StoreTags(context);
        
        return ValueTask.CompletedTask;
    }
    
    ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;
        
        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        if (response.StatusCode != StatusCodes.Status200OK)
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        return ValueTask.CompletedTask;
    }

    private void StoreTags(OutputCacheContext context)
    {
        string? stockTag = null;
        foreach (var (key, value) in context.HttpContext.Request.RouteValues)
        {
            if (key != "id") continue;
            stockTag = value as string;
            break;
        }

        if (string.IsNullOrEmpty(stockTag)) return; 
        context.Tags.Add(stockTag);
        context.Tags.Add(CacheTags.STOCK_SINGLE);
    }
    
    private bool AttemptOutputCaching(OutputCacheContext context)
    {
        var request = context.HttpContext.Request;

        if (!HttpMethods.IsGet(request.Method))
        {
            return false;
        }
        
        return true;
    }
}