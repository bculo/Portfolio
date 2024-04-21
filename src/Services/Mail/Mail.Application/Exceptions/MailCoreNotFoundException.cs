namespace Mail.Application.Exceptions;

public class MailCoreNotFoundException : MailCoreException
{
    public string UserMessage { get; set; } = default!;

    public MailCoreNotFoundException(string message = "Item not found") : base(message)
    {
        UserMessage = message;
    }
}