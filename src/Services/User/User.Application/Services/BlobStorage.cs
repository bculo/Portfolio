using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.Application.Common.Options;
using User.Application.Interfaces;

namespace User.Application.Services;

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
            logger.LogWarning("Container {ContainerName} already created", containerName);
        }
    }

    private BlobClient GetBlobClient(string containerName, string blobIdentifier)
    {
        return blobClient.GetBlobContainerClient(containerName)
            .GetBlobClient(blobIdentifier);
    }

    public async Task<Uri> UploadBlob(string containerName, string blobIdentifier, Stream blob, string contentType)
    {
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType,
            ContentLanguage = "en"
        };
        
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        await blobClient.UploadAsync(blob, httpHeaders: httpHeaders);
        return blobClient.Uri;
    }
}