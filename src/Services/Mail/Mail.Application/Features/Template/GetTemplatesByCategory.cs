using Mail.Application.Entities.Enums;
using Mail.Application.Models;
using Mail.Application.Services.Interfaces;
using MediatR;

namespace Mail.Application.Features.Template;

public static class GetTemplatesByCategory
{
    public class GTBCQuery : IRequest<IEnumerable<MailTemplateBaseDto>>
    {
        public MailTemplateCategory Category { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class GTBCHandler : IRequestHandler<GTBCQuery, IEnumerable<MailTemplateBaseDto>>
    {
        private readonly IMailTemplateRepository _repo;

        public GTBCHandler(IMailTemplateRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MailTemplateBaseDto>> Handle(GTBCQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetTemplatesByCategory(request.Category, request.IsActive);
            return result.Select(i => i.ToBaseDto());
        }
    }
}