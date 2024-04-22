using FluentValidation;
using Mail.Application.Entities.Enums;
using Mail.Application.Exceptions;
using Mail.Application.Models;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features.Template;

public static class GetSingleTemplate
{
    public class Query : IRequest<MailTemplateDetailsDto>
    {
        public MailTemplateCategory Category { get; set; }
        public string Name { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(i => i.Name)
                .NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, MailTemplateDetailsDto>
    {
        private readonly IMailTemplateRepository _repo;
        private readonly ILogger<Handler> _logger;
        
        public Handler(IMailTemplateRepository repo, ILogger<Handler> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        public async Task<MailTemplateDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetSingle(request.Category, request.Name, cancellationToken);

            if (result is null)
            {
                _logger.LogWarning("Item with PK {0} and SK {0} not found", request.Category, request.Name);
                throw new MailCoreNotFoundException();
            }

            return result.ToDetailsDto();
        }
    }
}