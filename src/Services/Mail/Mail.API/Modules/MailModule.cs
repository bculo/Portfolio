using Carter;
using Mail.Application.Features;
using Mail.Application.Features.Mail;
using MediatR;

namespace Mail.API.Modules;

public class MailModule : ICarterModule
{
    private const string MODULE_NAME = "Mail";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{MODULE_NAME}/InvokeSendMailProcedure", 
            async (InvokeSendMailProcedure.ISMPCommand request, IMediator mediator) =>
        {
            await mediator.Send(request);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}

