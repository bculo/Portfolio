using Mail.Application.Interfaces;
using Mail.Application.Interfaces.Repository;
using Mail.Application.Models;
using MediatR;

namespace Mail.Application.Features.Template;

public static class GetTemplates
{
    public class Query : IRequest<IEnumerable<MailTemplateBaseDto>> { }

    public class Handler : IRequestHandler<Query, IEnumerable<MailTemplateBaseDto>>
    {
        private readonly IMailTemplateRepository _context;

        public Handler(IMailTemplateRepository context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<MailTemplateBaseDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var response = await _context.GetAll(cancellationToken);
            return response.Select(i => i.ToBaseDto());
        }
    }
}