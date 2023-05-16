using Events.Common.Mail;
using Mail.Application.Services.Interfaces;
using MassTransit;
using Time.Common.Contracts;

namespace Mail.Application.Consumers;

public class SendCustomMailConsumer : IConsumer<SendCustomMail>
{
    private readonly IEmailService _mailService;
    private readonly IDateTimeProvider _timeProvider;
    
    public SendCustomMailConsumer(IEmailService mailService, IDateTimeProvider timeProvider)
    {
        _mailService = mailService;
        _timeProvider = timeProvider;
    }

    public async Task Consume(ConsumeContext<SendCustomMail> context)
    {
        var mailInstance = context.Message;
        
        await _mailService.SendMail(mailInstance.From, mailInstance.To, mailInstance.Title,mailInstance.Message);
        
        //TODO -> outbox pattern needed (check dynamoDB transactions)?
        //TODO -> insert sent message to dynamoDB Mail table
        
        await context.Publish(new CustomMailSent
        {
            MailId = Guid.NewGuid().ToString(),
            SentDate = _timeProvider.Now
        });
    }
} 