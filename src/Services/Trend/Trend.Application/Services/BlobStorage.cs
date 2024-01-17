using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;

namespace Trend.Application.Services;

public class BlobStorage : IBlobStorage
{
    private readonly ILogger<BlobStorage> _logger;
    private readonly BlobServiceClient _blobClient;
    private readonly BlobStorageOptions _options;
    
    public BlobStorage(BlobServiceClient blobClient, 
        IOptions<BlobStorageOptions> options,
        ILogger<BlobStorage> logger)
    {
        _blobClient = blobClient;
        _logger = logger;
        _options = options.Value;
    }

    public void InitializeContext(string containerName, bool isPublic = false)
    {
        try
        {
            var accessType = isPublic ? PublicAccessType.Blob : PublicAccessType.None;
            _blobClient.CreateBlobContainer(containerName, accessType);
        }
        catch
        {
            _logger.LogInformation("Container {ContainerName} already created", containerName);
        }
    }

    private BlobClient GetBlobClient(string containerName, string blobIdentifier)
    {
        return _blobClient.GetBlobContainerClient(containerName)
            .GetBlobClient(blobIdentifier);
    }

    public Uri GetBaseUri => _blobClient.Uri;

    public async Task<Uri> UploadBlobAsync(string containerName, string blobIdentifier, Stream blob, string contentType)
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

    public Uri UploadBlob(string containerName, string blobIdentifier, Stream blob, string contentType)
    {
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType,
            ContentLanguage = "en"
        };
        
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        blobClient.Upload(blob, httpHeaders: httpHeaders);
        return blobClient.Uri;
    }

    public Task<bool> ExistsAsync(string containerName, string blobIdentifier)
    {
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        return Task.FromResult(blobClient.Exists().Value);
    }

    public bool Exists(string containerName, string blobIdentifier)
    {
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        return blobClient.Exists().Value;
    }
}