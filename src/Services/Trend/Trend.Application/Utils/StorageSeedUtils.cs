using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;
using Trend.Resources.BlobDefaultImages;

namespace Trend.Application.Utils;

public static class StorageSeedUtils
{
    public static async Task SeedBlobStorage(IBlobStorage blobStorage, IOptions<BlobStorageOptions> blobOptions)
    {
        List<DefaultBlobInfo> defaultBlobs = new()
        {
            ResourceBlobMap.GetImageInfo(ContextType.Crypto.DisplayValue),
            ResourceBlobMap.GetImageInfo(ContextType.Stock.DisplayValue),
            ResourceBlobMap.GetImageInfo(ContextType.Forex.DisplayValue),
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
            await blobStorage.Upload(blobOptions.Value.TrendContainerName,
                defaultBlob.BlobName,
                stream,
                defaultBlob.ContentType);
        }
    }
}