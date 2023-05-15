using Carter;
using Mail.Application.Features.Template;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mail.API.Modules;

public class TemplateModule : ICarterModule
{
    private const string MODULE_NAME = "Template";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{MODULE_NAME}/AddTemplate", 
            async ([FromBody] AddTemplate.ATCommand request, IMediator mediator) =>
            {
                await mediator.Send(request);
                return Results.NoContent();
            }).RequireAuthorization();

        app.MapGet($"{MODULE_NAME}/GetTemplates", async (IMediator mediator) =>
        {
            return Results.Ok(await mediator.Send(new GetTemplates.GTQuery()));
        }).RequireAuthorization();
        
        app.MapPost($"{MODULE_NAME}/GetTemplatesForCategory", 
            async ([FromBody] GetTemplatesByCategory.GTBCQuery query, IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(query));
            }).RequireAuthorization();
        
        app.MapPost($"{MODULE_NAME}/GetSingleTemplate",
            async ([FromBody] GetSingleTemplate.GSTQuery request, IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(request));
            }).RequireAuthorization();
        
        app.MapPost($"{MODULE_NAME}/DeactivateTemplate",
            async ([FromBody] DeactivateTemplate.DTCommand request, IMediator mediator) =>
            {
                await mediator.Send(request);
                return Results.NoContent();
            }).RequireAuthorization();
    }
}