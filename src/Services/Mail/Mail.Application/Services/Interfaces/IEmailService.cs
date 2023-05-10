namespace Mail.Application.Services.Interfaces;

public interface IEmailService
{
    public Task SendMail(string emailFrom, string emailTo, string body);
}