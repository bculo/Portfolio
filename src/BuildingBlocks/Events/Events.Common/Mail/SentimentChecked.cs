namespace Events.Common.Mail;

public class SentimentChecked
{
    public float Score { get; set; }
    public int NumberOfStars { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string FromMail { get; set; } = default!;
}