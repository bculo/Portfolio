namespace Trend.Application.Configurations.Options;

public class BlobStorageOptions
{
    public string TrendContainerName { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
}