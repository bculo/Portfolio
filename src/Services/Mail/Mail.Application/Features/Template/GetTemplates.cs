using Amazon.DynamoDBv2.DataModel;
using Mail.Application.Entities;
using MediatR;

namespace Mail.Application.Features.Template;

public static class GetTemplates
{
    public class GTQuery : IRequest<IEnumerable<GTResponse>> { }

    public class GTHandler : IRequestHandler<GTQuery, IEnumerable<GTResponse>>
    {
        private readonly IDynamoDBContext _context;

        public GTHandler(IDynamoDBContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<GTResponse>> Handle(GTQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.ScanAsync<MailTemplate>(null).GetRemainingAsync(cancellationToken);

            throw new NotImplementedException();
            //return response.Select(i => i.ToDto());
        }
    }

    public class GTResponse
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
        public string Template { get; set; }
        public DateTime Created { get; set; }
    }

    private static GTResponse ToDto(this MailTemplate entity)
    {
        /*
        return new GTResponse
        {
            TemplateName = entity.Name,
            Created = entity.Created,
            Template = entity.Content,
            Id = Guid.Parse(entity.Id)
        };
        */

        return null;
    }
}