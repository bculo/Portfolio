using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Time.Abstract.Contracts;

namespace Stock.Application.Commands.Cache;

public record EvictAllOutputCache() : IRequest;

public class EvictAllOutputCacheHandler : IRequestHandler<EvictAllOutputCache>
{
    private readonly IOutputCacheStore _outputCache;
    private readonly IDateTimeProvider _timeProvider;
    private readonly ILogger<EvictAllOutputCacheHandler> _logger;

    public EvictAllOutputCacheHandler(IOutputCacheStore outputCache, 
        ILogger<EvictAllOutputCacheHandler> logger, 
        IDateTimeProvider timeProvider)
    {
        _outputCache = outputCache;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task Handle(EvictAllOutputCache request, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Cleaning output cache. Time {Time}", _timeProvider.Time);
        await _outputCache.EvictByTagAsync(CacheTags.All, cancellationToken);
    }
}