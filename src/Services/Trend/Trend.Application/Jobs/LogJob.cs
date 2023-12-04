using Microsoft.Extensions.Logging;

namespace Trend.Application.Jobs;

public class LogJob
{
    private readonly ILogger<LogJob> _logger;

    public LogJob(ILogger<LogJob> logger)
    {
        _logger = logger;
    }

    public void Log()
    {
        _logger.LogInformation("HELLO");
    }
}