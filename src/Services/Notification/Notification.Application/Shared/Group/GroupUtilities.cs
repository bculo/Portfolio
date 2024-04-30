namespace Notification.Application.Shared.Group;

public static class GroupUtilities
{
    public static string FormatGroupName(string groupName, string groupType)
    {
        ArgumentException.ThrowIfNullOrEmpty(groupName);

        return $"{groupName.ToLower()}-{groupType.ToLower()}";
    }
}