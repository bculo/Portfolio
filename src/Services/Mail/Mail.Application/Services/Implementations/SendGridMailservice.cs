using Mail.Application.Options;
using Mail.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Mail.Application.Services.Implementations;

public class SendGridMailservice : IEmailService
{
    private readonly MailOptions _options;
    private readonly ILogger<SendGridMailservice> _logger;

    public SendGridMailservice(IOptions<MailOptions> options,
        ILogger<SendGridMailservice> logger)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task SendMail(string emailFrom, string emailTo, string subject, string body)
    {
        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress(emailFrom);
        var to = new EmailAddress(emailTo);
        
        var plainTextContent = body;
        var htmlContent = $"<strong>{body}</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        
        var response = await client.SendEmailAsync(msg);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(response.StatusCode.ToString());
        }
    }
}