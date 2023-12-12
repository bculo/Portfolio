namespace Events.Common.User;

public class UserImageVerified
{
    public long UserId { get; set; }
    public bool IsPerson { get; set; }
}