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

    public void InitializeContext()
    {
        try
        {
            _blobClient.CreateBlobContainer(_options.ContainerName);
        }
        catch
        {
            _logger.LogWarning("Container {0} already created", _options.ContainerName);
        }
    }

    private BlobClient GetBlobClient(string blobIdentifier)
    {
        return _blobClient.GetBlobContainerClient(_options.ContainerName)
            .GetBlobClient(blobIdentifier);
    }

    public async Task UploadBlob(string blobIdentifier, byte[] blob, string contentType)
    {
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType,
            ContentLanguage = "en"
        };
        
        using var stream = new MemoryStream(blob);
        var blobClient = GetBlobClient(blobIdentifier);
        await blobClient.UploadAsync(stream, httpHeaders: httpHeaders);
    }
}