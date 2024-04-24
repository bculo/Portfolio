using Auth0.Abstract.Contracts;
using Events.Common.Mail;
using Events.MassTransit.Common.Serializers;
using FluentValidation;
using Mail.Application.Options;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;

namespace Mail.Application.Features.Mail;

public static class InvokeSendMailProcedure
{
    public class Command : IRequest
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(i => i.Message)
                .NotEmpty();
            
            RuleFor(i => i.Title)
                .NotEmpty();
        }
    }
    public class Handler : IRequestHandler<Command>
    {
        private readonly IPublishEndpoint _publisher;
        private readonly IAuth0AccessTokenReader _tokenReader;
        private readonly MailOptions _mailOptions;

        public Handler(IPublishEndpoint publisher,
            IAuth0AccessTokenReader tokenReader,
            IOptions<MailOptions> options)
        {
            _publisher = publisher;
            _tokenReader = tokenReader;
            _mailOptions = options.Value;
        }
        
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var body = new CheckMailSentimentBody
            {
                Message = request.Message,
                Title = request.Title,
                From = _tokenReader.GetEmail(),
                To = _mailOptions.AppSupportMail,
                UserId = _tokenReader.GetIdentifier().ToString()
            };
            
            var messageEnvelope = new CheckMailSentiment
            {
                Body = body,
                RawMessage = body,
                MessageId = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid()
            };
            
            await _publisher.Publish(messageEnvelope, x =>
            {
                x.Serializer = new SystemTextJsonCustomRawMessageSerializer(RawSerializerOptions.All);
                x.MessageId = messageEnvelope.MessageId;
                x.CorrelationId = messageEnvelope.CorrelationId;
            }, cancellationToken);
        }
    }
}