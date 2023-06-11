using Events.Common.Mail;
using Mail.Application.Services.Interfaces;
using MassTransit;
using Time.Abstract.Contracts;

namespace Mail.Application.Consumers;

public class SendCustomMailConsumer : IConsumer<SendCustomMail>
{
    private readonly IEmailService _mailService;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IMailRepository _mailRepo;

    public SendCustomMailConsumer(IEmailService mailService, 
        IDateTimeProvider timeProvider,
        IMailRepository mailRepo)
    {
        _mailService = mailService;
        _timeProvider = timeProvider;
        _mailRepo = mailRepo;
    }

    public async Task Consume(ConsumeContext<SendCustomMail> context)
    {
        var mailInstance = context.Message;
        
        await _mailService.SendMail(mailInstance.From, mailInstance.To, mailInstance.Title,mailInstance.Message);
        
        //TODO -> outbox pattern needed (check dynamoDB transactions)?
        
        var entityModel = MapToEntity(mailInstance);
        await _mailRepo.AddItem(entityModel);
        
        await context.Publish(new CustomMailSent
        {
            MailId = Guid.NewGuid().ToString(),
            SentDate = _timeProvider.Now,
            UserId = mailInstance.UserId,
        });
    }

    private Entities.Mail MapToEntity(SendCustomMail customMail)
    {
        return new Entities.Mail()
        {
            Id = Guid.NewGuid().ToString(),
            To = customMail.To,
            From = customMail.From,
            Title = customMail.Title,
            IsActive = true,
            Body = customMail.Message,
            Created = _timeProvider.Now,
            UserId = customMail.UserId
        };
    }
} 