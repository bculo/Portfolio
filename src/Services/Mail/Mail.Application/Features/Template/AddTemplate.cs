using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using FluentValidation;
using Mail.Application.Entities;
using Mail.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Time.Common.Contracts;

namespace Mail.Application.Features;

public static class AddTemplate
{
    public class ATCommand : IRequest
    {
        public string TemplateName { get; set; }
        public string Template { get; set; }
    }

    public class Validator : AbstractValidator<ATCommand>
    {
        public Validator()
        {
            RuleFor(i => i.Template)
                .NotEmpty();

            RuleFor(i => i.TemplateName)
                .NotEmpty();
        }
    }

    public class Handler : IRequestHandler<ATCommand>
    {
        private readonly IDynamoDBContext _context;
        private readonly IDateTimeProvider _time;
        
        public Handler(IDynamoDBContext context, IDateTimeProvider time)
        {
            _time = time;
            _context = context;
        }
        
        public async Task Handle(ATCommand request, CancellationToken cancellationToken)
        {
            var conditions = new List<ScanCondition>
            {
                new ("Name", ScanOperator.Equal, request.TemplateName)
            };

            var ressult = await _context.ScanAsync<MailTemplate>(conditions).GetRemainingAsync(cancellationToken);
            
            if (ressult.Any())
            {
                throw new MailCoreException("Template with same name already exists");
            }
            
            await _context.SaveAsync(new MailTemplate
            {
                Content = request.TemplateName,
                Name = request.Template,
                Id = Guid.NewGuid().ToString(),
                Created = _time.Now
            });
        }
    }
}