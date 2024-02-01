namespace Events.Common.Mail;

public class SendCustomMail
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string UserId { get; set; } = default!;
}