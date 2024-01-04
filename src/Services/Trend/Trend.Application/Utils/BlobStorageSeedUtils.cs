using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;
using Trend.Resources.BlobDefaultImages;

namespace Trend.Application.Utils;

public static class BlobStorageSeedUtils
{
    public static async Task SeedBlobStorage(IServiceCollection services)
    {
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();
        var blobOptions = scope.ServiceProvider.GetRequiredService<IOptions<BlobStorageOptions>>();

        List<DefaultBlobInfo> defaultBlobs = new()
        {
            ResourceBlobMap.GetImagePath(ContextType.Crypto.ToString()),
            ResourceBlobMap.GetImagePath(ContextType.Stock.ToString()),
            ResourceBlobMap.GetImagePath(ContextType.Etf.ToString()),
            ResourceBlobMap.GetImagePath(ContextType.Economy.ToString()),
        };

        foreach (var defaultBlob in defaultBlobs)
        {
            if (!File.Exists(defaultBlob.FullPath))
            {
                continue;
            }
            
            if (await blobStorage.Exists(blobOptions.Value.TrendContainerName, defaultBlob.BlobName))
            {
                continue;
            }

            using var stream = new MemoryStream();
            await using var fStream = new FileStream(defaultBlob.FullPath, FileMode.Open);
            
            await fStream.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            await blobStorage.UploadBlob(blobOptions.Value.TrendContainerName,
                defaultBlob.BlobName,
                stream,
                defaultBlob.ContentType);
 
        }
    }
}