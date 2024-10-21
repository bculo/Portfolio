using Carter;
using Mail.Application.Features;
using Mail.Application.Features.Mail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mail.API.Modules;

public class MailModule : ICarterModule
{
    private const string ModuleName = "Mail";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/v1")
            .MapPost($"{ModuleName}/InvokeSendMailProcedure", 
            async ([FromBody] InvokeSendMailProcedure.Command request, IMediator mediator, CancellationToken ct) =>
                {
                    await mediator.Send(request, ct);
                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithTags(ModuleName);
        
        app.MapGroup("/v1")
            .MapGet($"{ModuleName}/GetSingleMail", 
                async ([AsParameters] GetSingleMail.Query request, IMediator mediator, CancellationToken ct) =>
                {
                    return Results.Ok(await mediator.Send(request, ct));
                })
            .RequireAuthorization()
            .WithTags(ModuleName);
        
        app.MapGroup("/v1")
            .MapGet($"{ModuleName}/GetUserMail", 
                async (IMediator mediator, CancellationToken ct) =>
                {
                    return Results.Ok(await mediator.Send(new GetUserMails.Query(), ct));
                })
            .RequireAuthorization()
            .WithTags(ModuleName);
    }
}

