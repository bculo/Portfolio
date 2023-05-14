using Amazon.DynamoDBv2.DataModel;
using Mail.Application.Entities;
using Mail.Application.Models;
using Mail.Application.Services.Interfaces;
using MediatR;

namespace Mail.Application.Features.Template;

public static class GetTemplates
{
    public class GTQuery : IRequest<IEnumerable<MailTemplateBaseDto>> { }

    public class GTHandler : IRequestHandler<GTQuery, IEnumerable<MailTemplateBaseDto>>
    {
        private readonly IMailTemplateRepository _context;

        public GTHandler(IMailTemplateRepository context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<MailTemplateBaseDto>> Handle(GTQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.GetAll();
            return response.Select(i => i.ToBaseDto());
        }
    }
}