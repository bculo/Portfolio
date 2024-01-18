namespace Trend.Application.Interfaces;

public interface IBlobStorage
{
    Uri GetBaseUri { get; }
    Task<Uri> UploadBlobAsync(string containerName, string blobIdentifier, Stream blob,  string contentType, CancellationToken token = default);
    Uri UploadBlob(string containerName, string blobIdentifier, Stream blob, string contentType);
    Task<bool> ExistsAsync(string containerName, string blobIdentifier, CancellationToken token = default);
    bool Exists(string containerName, string blobIdentifier);
}