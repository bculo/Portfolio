using Mail.Application.Interfaces.Mail;
using Mail.Application.Interfaces.Mail.Models;
using Mail.Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Mail.Infrastructure.Mail;

public class SendGridMailService : IEmailService
{
    private readonly MailOptions _options;
    private readonly ILogger<SendGridMailService> _logger;

    public SendGridMailService(IOptions<MailOptions> options,
        ILogger<SendGridMailService> logger)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task SendMail(SendMailModel model, CancellationToken token)
    {
        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress(model.From);
        var to = new EmailAddress(model.To);
        
        var plainTextContent = model.Content;
        var htmlContent = $"<strong>{model.Content}</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, model.Title, plainTextContent, htmlContent);
        
        var response = await client.SendEmailAsync(msg, token);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(response.StatusCode.ToString());
        }
    }
}