using Auth0.Abstract.Contracts;
using FluentValidation;
using Mail.Application.Entities;
using Mail.Application.Entities.Enums;
using Mail.Application.Interfaces;
using Mail.Application.Interfaces.Repository;
using MediatR;
using Time.Abstract.Contracts;

namespace Mail.Application.Features.Template;

public static class AddTemplate
{
    public class Command : IRequest
    {
        public string TemplateName { get; set; } = default!;
        public string Template { get; set; } = default!;
        public string Title { get; set; } = default!;
        public MailTemplateCategory Category { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(i => i.Template)
                .NotEmpty();

            RuleFor(i => i.TemplateName)
                .NotEmpty();

            RuleFor(i => i.Title)
                .NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IMailTemplateRepository _repo;
        private readonly IDateTimeProvider _time;
        private readonly IAuth0AccessTokenReader _tokenReader;
        
        public Handler(IMailTemplateRepository repo, 
            IDateTimeProvider time, 
            IAuth0AccessTokenReader reader)
        {
            _time = time;
            _repo = repo;
            _tokenReader = reader;
        }
        
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var item = new MailTemplate
            {
                Category = (int)request.Category,
                Template = request.TemplateName,
                Name = request.TemplateName,
                Title = request.Title,
                Created = _time.Now,
                IsActive = true,
                ModifiedBy = _tokenReader.GetIdentifier().ToString(),
                ModificationDate = _time.Now
            }; 
            
            await _repo.AddItem(item, cancellationToken);
        }
    }
}