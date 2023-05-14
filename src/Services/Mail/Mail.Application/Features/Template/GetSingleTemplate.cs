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
    public class GSTQuery : IRequest<MailTemplateDetailsDto>
    {
        public MailTemplateCategory Category { get; set; }
        public string Name { get; set; }
    }

    public class GSTValidator : AbstractValidator<GSTQuery>
    {
        public GSTValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty();
        }
    }

    public class GSTHandler : IRequestHandler<GSTQuery, MailTemplateDetailsDto>
    {
        private readonly IMailTemplateRepository _repo;
        private readonly ILogger<GSTHandler> _logger;
        
        public GSTHandler(IMailTemplateRepository repo, ILogger<GSTHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        
        public async Task<MailTemplateDetailsDto> Handle(GSTQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetSingle(request.Category, request.Name);

            if (result is null)
            {
                _logger.LogWarning("Item with PK {0} and SK {0} not found", request.Category, request.Name);
                throw new MailCoreNotFoundException();
            }

            return result.ToDetailsDto();
        }
    }
}