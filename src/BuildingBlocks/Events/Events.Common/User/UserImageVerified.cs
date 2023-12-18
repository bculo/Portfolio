namespace Events.Common.User;

public class UserImageVerified
{
    public Guid UserId { get; set; }
    public bool IsPerson { get; set; }
    public bool IsNsfw { get; set; }
    public string UserName { get; set; }
}