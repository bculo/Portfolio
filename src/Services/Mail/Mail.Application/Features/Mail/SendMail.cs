using FluentValidation;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features.Mail;

public static class SendMail
{
    public class SMCommand : IRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }

    public class Validator : AbstractValidator<SMCommand>
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
        }
    }
    public class Handler : IRequestHandler<SMCommand>
    {
        private readonly IEmailService _mail;
        private readonly ILogger<Handler> _logger;
        
        public Handler(ILogger<Handler> logger, IEmailService mail)
        {
            _mail = mail;
            _logger = logger;
        }
        
        public async Task Handle(SMCommand request, CancellationToken cancellationToken)
        {
            await _mail.SendMail(request.From, request.To, request.Message);
        }
    }
}