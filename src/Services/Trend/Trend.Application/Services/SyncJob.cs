using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;

namespace Trend.Application.Services;

public class SyncJob : ISyncJob
{
    private readonly ILogger<SyncJob> _logger;
    private readonly ISyncService _syncService;

    public SyncJob(ISyncService syncService, ILogger<SyncJob> logger)
    {
        _syncService = syncService;
        _logger = logger;
    }


    public async Task Work(CancellationToken token)
    {
        await _syncService.ExecuteSync(token);
    }
}