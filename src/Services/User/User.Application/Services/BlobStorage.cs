using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MassTransit.Initializers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.Application.Common.Options;
using User.Application.Interfaces;

namespace User.Application.Services;

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
            _logger.LogWarning("Container {ContainerName} already created", containerName);
        }
    }

    private BlobClient GetBlobClient(string containerName, string blobIdentifier)
    {
        return _blobClient.GetBlobContainerClient(containerName)
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