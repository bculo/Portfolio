
using Auth0.Abstract.Contracts;
using Mail.Application.Exceptions;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features.Mail;

public static class GetSingleMail
{
    public class Query : IRequest<Response>
    {
        public string MailId { get; set; }
    }
    
    public class Response
    {
        public string To { get; set; }
        public string Form { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IAuth0AccessTokenReader _tokenReader;
        private readonly IMailRepository _mailRepo;
        private readonly ILogger<Handler> _logger;

        public Handler(IAuth0AccessTokenReader tokenReader,
            IMailRepository mailRepo,
            ILogger<Handler> logger)
        {
            _tokenReader = tokenReader;
            _mailRepo = mailRepo;
            _logger = logger;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _tokenReader.GetIdentifier().ToString();
            var entity = await _mailRepo.GetSingle(userId, request.MailId);
            
            if (entity is null)
            {
                _logger.LogWarning("Item with PK {0} and SK {0} not found", userId, request.MailId);
                throw new MailCoreNotFoundException();
            }

            return MapToDto(entity);
        }
        
        private Response MapToDto(Entities.Mail entity)
        {
            return new Response
            {
                Title = entity.Title,
                Form = entity.From,
                To = entity.To,
                Message = entity.Body
            };
        }
    }
}