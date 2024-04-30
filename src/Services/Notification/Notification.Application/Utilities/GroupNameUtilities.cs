namespace Notification.Application.Utilities;

public static class GroupNameUtilities
{
    public static string FormatGroupName(string groupName, string assetType)
    {
        ArgumentException.ThrowIfNullOrEmpty(groupName);
        ArgumentException.ThrowIfNullOrEmpty(assetType);

        return $"{groupName}-{assetType}";
    }
}