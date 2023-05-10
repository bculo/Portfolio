namespace Mail.Application.Exceptions;

public class MailCoreException : Exception
{
    public string UserMessage { get; set; }

    public MailCoreException(string message) : base(message)
    {
        UserMessage = message;
    }

    public MailCoreException(string userMessage, string devMessage) : base(devMessage)
    {
        UserMessage = userMessage;
    }

    public static void ThrowIfNull(object instance, string message)
    {
        if (instance is null)
        {
            throw new MailCoreException(message);
        }
    }

    public static void ThrowIfEmpty<TType>(IEnumerable<TType> values, string message)
    {
        if (values is null || !values.Any())
        {
            throw new MailCoreException(message);
        }
    }
}