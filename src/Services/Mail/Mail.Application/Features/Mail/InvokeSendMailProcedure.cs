using Events.Common.Mail;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Mail.Application.Features.Mail;

public static class InvokeSendMailProcedure
{
    public class ISMPCommand : IRequest
    {
        public string From { get; set; }
        public string To { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class ISMPValidator : AbstractValidator<ISMPCommand>
    {
        public ISMPValidator()
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
    public class ISMPHandler : IRequestHandler<ISMPCommand>
    {
        private readonly IPublishEndpoint _publisher;

        public ISMPHandler(IPublishEndpoint publisher)
        {
            _publisher = publisher;
        }
        
        public async Task Handle(ISMPCommand request, CancellationToken cancellationToken)
        {
            var command = new SendCustomMail()
            {
                Message = request.Message,
                Title = request.Title,
                From = request.From,
                To = request.To
            };

            await _publisher.Publish(command);
        }
    }
}