namespace Events.Common.Mail;

public class SendCustomMail
{
    public string From { get; set; }
    public string To { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
}