namespace Trend.Application.Interfaces;

public interface IBlobStorage
{
    Task<Uri> UploadBlob(string containerName, string blobIdentifier, Stream blob,  string contentType);
    Task<bool> Exists(string containerName, string blobIdentifier);
}