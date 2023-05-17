using Mail.Application.Entities.Enums;
using Mail.Application.Models;
using Mail.Application.Services.Interfaces;
using MediatR;

namespace Mail.Application.Features.Template;

public static class GetTemplatesByCategory
{
    public class Query : IRequest<IEnumerable<MailTemplateBaseDto>>
    {
        public MailTemplateCategory Category { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class Handler : IRequestHandler<Query, IEnumerable<MailTemplateBaseDto>>
    {
        private readonly IMailTemplateRepository _repo;

        public Handler(IMailTemplateRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MailTemplateBaseDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetTemplatesByCategory(request.Category, request.IsActive);
            return result.Select(i => i.ToBaseDto());
        }
    }
}