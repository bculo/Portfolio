using Carter;
using Mail.Application.Features;
using Mail.Application.Features.Mail;
using Mail.Application.Features.Template;
using MediatR;

namespace Mail.API.Modules;

public class TemplateModule : ICarterModule
{
    private const string MODULE_NAME = "Template";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{MODULE_NAME}/AddTemplate", async (AddTemplate.ATCommand request, IMediator mediator) =>
        {
            await mediator.Send(request);
            return Results.NoContent();
        });
        
        app.MapGet($"{MODULE_NAME}/GetTemplates", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new GetTemplates.GTQuery()));
        });
    }
}