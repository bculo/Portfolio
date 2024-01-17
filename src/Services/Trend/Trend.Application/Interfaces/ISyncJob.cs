namespace Trend.Application.Interfaces;

public interface ISyncJob
{
    Task Work(CancellationToken token);
}