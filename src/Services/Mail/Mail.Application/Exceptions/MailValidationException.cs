namespace Mail.Application.Exceptions;

public class MailValidationException : MailCoreException
{
    public Dictionary<string, string[]> Errors { get; set; }

    public MailValidationException(Dictionary<string, string[]> errors) : base("Validation exception")
    {
        Errors = errors;
    }
}