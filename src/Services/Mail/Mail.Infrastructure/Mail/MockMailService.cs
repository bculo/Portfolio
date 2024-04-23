using Mail.Application.Interfaces.Mail;
using Mail.Application.Interfaces.Mail.Models;
using Microsoft.Extensions.Logging;

namespace Mail.Infrastructure.Mail;

public class MockMailService : IEmailService
{
    private readonly ILogger<MockMailService> _logger;
    
    public MockMailService(ILogger<MockMailService> logger)
    {
        _logger = logger;
    }
    
    public async Task SendMail(SendMailModel model, CancellationToken token)
    {
        _logger.LogWarning("Using mock mail service");
        await Task.Delay(100, token);
    }
}