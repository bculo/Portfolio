namespace Mail.Application.Options;

public sealed class MailOptions
{
    public string ApiKey { get; set; } = default!;
    public string AppSupportMail { get; set; } = default!;
}