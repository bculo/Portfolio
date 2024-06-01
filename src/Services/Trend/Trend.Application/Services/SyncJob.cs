using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;

namespace Trend.Application.Services;

public class SyncJob(ISyncService syncService, ILogger<SyncJob> logger) : ISyncJob
{
    private readonly ILogger<SyncJob> _logger = logger;


    public async Task Work(CancellationToken token = default)
    {
        await syncService.ExecuteSync(token);
    }
}