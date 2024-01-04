namespace Trend.Resources.BlobDefaultImages;

public static class ResourceBlobMap
{
    private static readonly Dictionary<string, string> ImageMap = new()
    {
        { "Crypto", "Crypto.jpeg" },
        { "Etf", "Etf.jpg" },
        { "Stock", "Stock.jpg" },
        { "Economy", "Economy.jpg" },
    };

    public static DefaultBlobInfo GetImagePath(string keyword)
    {
        if (!ImageMap.ContainsKey(keyword))
        {
            return null;
        }

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var fullPath = Path.Combine(baseDir, "BlobDefaultImages", ImageMap[keyword]);
        var contentType = ImageMap[keyword].EndsWith(".jpeg") ? "image/jpeg" : "image/jpg";
        return new DefaultBlobInfo(fullPath, keyword, contentType);
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

