using FluentValidation;
using Mail.Application.Entities.Enums;
using Mail.Application.Exceptions;
using Mail.Application.Interfaces;
using Mail.Application.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features.Template;

public static class DeactivateTemplate
{
    public class Command : IRequest
    {
        public MailTemplateCategory Category { get; set; }
        public string Name { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(i => i.Name)
                .NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IMailTemplateRepository _repo;
        private readonly ILogger<Handler> _logger;
     
        public Handler(IMailTemplateRepository repo,
            ILogger<Handler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetSingle(request.Category, request.Name, cancellationToken);

            if (item is null)
            {
                _logger.LogWarning("Item with PK {0} and SK {0} not found", request.Category, request.Name);
                throw new MailCoreNotFoundException();
            }

            item.IsActive = false;
            await _repo.UpdateItem(item, cancellationToken);
        }
    }
}