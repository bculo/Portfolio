using FluentValidation;
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
        private readonly ILogger<Handler> _logger;
        
        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }
        
        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
;        }
    }
}