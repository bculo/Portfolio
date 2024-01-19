namespace Trend.Application.Interfaces;

public interface IBlobStorage
{
    Uri GetBaseUri { get; }
    Task<Uri> Upload(string containerName, string blobIdentifier, Stream blob,  string contentType, CancellationToken token = default);
    Task<bool> Exists(string containerName, string blobIdentifier, CancellationToken token = default);
}