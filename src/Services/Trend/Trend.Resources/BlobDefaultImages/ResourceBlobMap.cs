namespace Trend.Resources.BlobDefaultImages;

public static class ResourceBlobMap
{
    private static readonly Dictionary<string, string> ImageMap = new()
    {
        { "Crypto", "Crypto.jpeg" },
        { "Forex", "Forex.jpeg" },
        { "Stock", "Stock.jpeg" },
    };

    public static DefaultBlobInfo GetImageInfo(string keyword)
    {
        if (!ImageMap.ContainsKey(keyword))
        {
            return null;
        }

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var fullPath = Path.Combine(baseDir, "BlobDefaultImages", ImageMap[keyword]);
        return new DefaultBlobInfo(fullPath, keyword, "image/jpeg");
    }
    
    public static bool IsAllowedName(string blobName)
    {
        if (!ImageMap.ContainsKey(blobName))
        {
            return true;
        }

        return false;
    }
}

