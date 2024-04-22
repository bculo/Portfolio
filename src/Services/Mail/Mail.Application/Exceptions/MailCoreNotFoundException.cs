namespace Mail.Application.Exceptions;

public class MailCoreNotFoundException : MailCoreException
{
    public MailCoreNotFoundException(string message = "Item not found") : base(message)
    {
        UserMessage = message;
    }
}