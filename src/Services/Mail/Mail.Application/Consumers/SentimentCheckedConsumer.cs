using Events.Common.Mail;
using Mail.Application.Entities.Enums;
using Mail.Application.Interfaces.Mail;
using Mail.Application.Interfaces.Mail.Models;
using Mail.Application.Interfaces.Repository;
using Mail.Application.Options;
using MassTransit;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;

namespace Mail.Application.Consumers;

public class SentimentCheckedConsumer : IConsumer<SentimentChecked>
{
    private readonly IEmailService _mailService;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IMailRepository _mailRepo;
    private readonly MailOptions _mailOptions;

    public SentimentCheckedConsumer(IEmailService mailService, 
        IDateTimeProvider timeProvider,
        IMailRepository mailRepo,
        IOptions<MailOptions> options)
    {
        _mailService = mailService;
        _timeProvider = timeProvider;
        _mailRepo = mailRepo;
        _mailOptions = options.Value;
    }
    
    public async Task Consume(ConsumeContext<SentimentChecked> context)
    {
        var mailInstance = context.Message;

        var emailModel = new SendMailModel
        {
            Content = mailInstance.Content,
            From = mailInstance.FromMail,
            Title = mailInstance.Title,
            To = _mailOptions.AppSupportMail
        };
        
        await _mailService.SendMail(emailModel, default);
        
        var entityModel = MapToEntity(mailInstance);
        await _mailRepo.AddItem(entityModel, default);
        
        await context.Publish(new CustomMailSent
        {
            MailId = Guid.NewGuid().ToString(),
            SentDate = _timeProvider.Now,
            UserId = mailInstance.UserId,
        });
    }
    
    private Entities.Mail MapToEntity(SentimentChecked customMail)
    {
        return new Entities.Mail()
        {
            Id = Guid.NewGuid().ToString(),
            To = _mailOptions.AppSupportMail,
            From = customMail.FromMail,
            Title = customMail.Title,
            Status = Status.Created,
            Body = customMail.Content,
            Created = _timeProvider.Now,
            UserId = customMail.UserId,
            Priority = customMail.Score < 1.5f ? Priority.High : Priority.Low,
            Sentiment = customMail.Score < 2.5f ? Sentiment.Negative : Sentiment.Positive
        };
    }
}


public class SentimentCheckedConsumerDefinition : ConsumerDefinition<SentimentCheckedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<SentimentCheckedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseRawJsonDeserializer();
    }
}