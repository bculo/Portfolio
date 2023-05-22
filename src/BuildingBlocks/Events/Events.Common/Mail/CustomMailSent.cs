namespace Events.Common.Mail;

public class CustomMailSent
{
    public string MailId { get; set; }
    public DateTime SentDate { get; set; }
    public string UserId { get; set; }
}