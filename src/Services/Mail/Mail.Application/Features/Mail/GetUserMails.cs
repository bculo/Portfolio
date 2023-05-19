using Auth0.Abstract.Contracts;
using Mail.Application.Exceptions;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Features.Mail;

public static class GetUserMails
{
    public class Query : IRequest<IEnumerable<Response>> { }
    
    public class Response
    {
        public string To { get; set; }
        public string Form { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string MailId { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, IEnumerable<Response>>
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

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _tokenReader.GetIdentifier().ToString();
            
            var entities = await _mailRepo.GetAllUserMails(userId);

            return entities.Select(i => MapToDto(i));
        }

        private Response MapToDto(Entities.Mail entity)
        {
            return new Response
            {
                Title = entity.Title,
                Form = entity.From,
                To = entity.To,
                Message = entity.Body,
                MailId = entity.Id,
                UserId = entity.UserId
            };
        }
    }
}