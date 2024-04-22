
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
        public string MailId { get; set; } = default!;
    }
    
    public class Response
    {
        public string To { get; set; } = default!;
        public string Form { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Title { get; set; } = default!;
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IAuth0AccessTokenReader _tokenReader;
        private readonly IMailRepository _mailRepo;

        public Handler(IAuth0AccessTokenReader tokenReader,
            IMailRepository mailRepo)
        {
            _tokenReader = tokenReader;
            _mailRepo = mailRepo;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _tokenReader.GetIdentifier().ToString();
            var entity = await _mailRepo.GetSingle(userId, request.MailId, cancellationToken);
            
            if (entity is null)
            {
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