using System.Data;
using FluentValidation;
using Mail.Application.Entities.Enums;
using Mail.Application.Exceptions;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features.Template;

public static class DeactivateTemplate
{
    public class DTCommand : IRequest
    {
        public MailTemplateCategory Category { get; set; }
        public string Name { get; set; }
    }

    public class DTValidator : AbstractValidator<DTCommand>
    {
        public DTValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty();
        }
    }

    public class DTHandler : IRequestHandler<DTCommand>
    {
        private readonly IMailTemplateRepository _repo;
        private readonly ILogger<DTHandler> _logger;
     
        public DTHandler(IMailTemplateRepository repo,
            ILogger<DTHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task Handle(DTCommand request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetSingle(request.Category, request.Name);

            if (item is null)
            {
                _logger.LogWarning("Item with PK {0} and SK {0} not found", request.Category, request.Name);
                throw new MailCoreNotFoundException();
            }

            item.IsActive = false;
            await _repo.UpdateItem(item);
        }
    }
}