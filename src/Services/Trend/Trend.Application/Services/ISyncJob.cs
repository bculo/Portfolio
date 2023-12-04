namespace Trend.Application.Services;

public interface ISyncJob
{
    Task Work(CancellationToken token);
}