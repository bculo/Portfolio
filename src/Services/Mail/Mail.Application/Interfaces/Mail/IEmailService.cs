using Mail.Application.Interfaces.Mail.Models;

namespace Mail.Application.Interfaces.Mail;

public interface IEmailService
{
    public Task SendMail(SendMailModel model, CancellationToken token);
}