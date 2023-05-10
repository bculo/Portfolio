using FluentValidation;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features;

public static class SendMail
{
    public class Command : IRequest<Unit>
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
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
        }
    }
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IEmailService _mail;
        private readonly ILogger<Handler> _logger;
        
        public Handler(ILogger<Handler> logger, IEmailService mail)
        {
            _mail = mail;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _mail.SendMail(request.From, request.To, request.Message);
            return Unit.Value;
;       }
    }
}