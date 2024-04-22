using Auth0.Abstract.Contracts;
using Events.Common.Mail;
using Events.MassTransit.Common.Serializers;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Mail.Application.Features.Mail;

public static class InvokeSendMailProcedure
{
    public class Command : IRequest
    {
        public string From { get; set; } = default!;
        public string To { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(i => i.From)
                .EmailAddress()
                .NotEmpty();
            
            RuleFor(i => i.To)
                .EmailAddress()
                .NotEmpty();
            
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

        public Handler(IPublishEndpoint publisher,
            IAuth0AccessTokenReader tokenReader)
        {
            _publisher = publisher;
            _tokenReader = tokenReader;
        }
        
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var body = new CheckMailSentimentBody
            {
                Message = request.Message,
                Title = request.Title,
                From = request.From,
                To = request.To,
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