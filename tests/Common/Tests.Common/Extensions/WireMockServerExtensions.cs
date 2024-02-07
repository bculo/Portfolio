using System.Net;
using Tests.Common.Interfaces.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Tests.Common.Extensions;

public static class WireMockServerExtensions
{
    public static Task ReturnBadRequest(this WireMockServer server, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.BadRequest)
            );

        return Task.CompletedTask;
    } 
    
    public static Task ReturnOk(this WireMockServer server, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.NoContent)
            );

        return Task.CompletedTask;
    } 
    
    public static Task ReturnWithTextOk(this WireMockServer server, string text, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create()
                    .WithBody(text)
                    .WithStatusCode(HttpStatusCode.OK)
            );

        return Task.CompletedTask;
    } 
    
    public static async Task ReturnWithTextOk(this WireMockServer server, ITextLoader loader, string withPath = "*")
    {
        var text = await loader.LoadAsync();
        
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create()
                    .WithBody(text)
                    .WithStatusCode(HttpStatusCode.OK)
            );
    } 
    
    public static Task ReturnWithJsonOk(this WireMockServer server, object instance, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(instance)
                    .WithStatusCode(HttpStatusCode.OK)
            );

        return Task.CompletedTask;
    } 
}