using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;

namespace Trend.Application.Services;

public class BlobStorage(
    BlobServiceClient blobClient,
    IOptions<BlobStorageOptions> options,
    ILogger<BlobStorage> logger)
    : IBlobStorage
{
    private readonly BlobStorageOptions _options = options.Value;

    public void InitializeContext(string containerName, bool isPublic = false)
    {
        try
        {
            var accessType = isPublic ? PublicAccessType.Blob : PublicAccessType.None;
            blobClient.CreateBlobContainer(containerName, accessType);
        }
        catch
        {
            logger.LogInformation("Container {ContainerName} already created", containerName);
        }
    }

    private BlobClient GetBlobClient(string containerName, string blobIdentifier)
    {
        return blobClient.GetBlobContainerClient(containerName)
            .GetBlobClient(blobIdentifier);
    }

    public Uri GetBaseUri => blobClient.Uri;

    public async Task<Uri> Upload(string containerName, string blobIdentifier, Stream blob, string contentType, CancellationToken token = default)
    {
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType,
            ContentLanguage = "en"
        };
        
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        await blobClient.UploadAsync(blob, httpHeaders: httpHeaders, cancellationToken: token);
        return blobClient.Uri;
    }
    
    public Task<bool> Exists(string containerName, string blobIdentifier, CancellationToken token = default)
    {
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        return Task.FromResult(blobClient.Exists().Value);
    }
}