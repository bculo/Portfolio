using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;
using Exception = System.Exception;

namespace Trend.Application.Jobs;

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
        try
        {
            await _syncService.ExecuteSync(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}