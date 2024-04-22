using Carter;
using Mail.Application.Features;
using Mail.Application.Features.Mail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mail.API.Modules;

public class MailModule : ICarterModule
{
    private const string MODULE_NAME = "Mail";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/v1")
            .MapPost($"{MODULE_NAME}/InvokeSendMailProcedure", 
            async ([FromBody] InvokeSendMailProcedure.Command request, IMediator mediator, CancellationToken ct) =>
                {
                    await mediator.Send(request, ct);
                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithTags(MODULE_NAME);
        
        app.MapGroup("/v1")
            .MapPost($"{MODULE_NAME}/GetSingleMail", 
                async ([FromBody] GetSingleMail.Query request, IMediator mediator, CancellationToken ct) =>
                {
                    return Results.Ok(await mediator.Send(request, ct));
                })
            .RequireAuthorization()
            .WithTags(MODULE_NAME);
        
        app.MapGroup("/v1")
            .MapGet($"{MODULE_NAME}/GetUserMail", 
                async (IMediator mediator, CancellationToken ct) =>
                {
                    return Results.Ok(await mediator.Send(new GetUserMails.Query(), ct));
                })
            .RequireAuthorization()
            .WithTags(MODULE_NAME);
    }
}

