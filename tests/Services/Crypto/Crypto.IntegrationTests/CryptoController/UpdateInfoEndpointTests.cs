using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands;
using Crypto.IntegrationTests.Helpers;
using Crypto.Shared.Builders;
using Crypto.Shared.Utilities;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class UpdateInfoEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnNotFound_WhenNonExistentSymbolProvided()
    {
        await Authenticate(UserRole.Admin);
        var request = new UpdateInfoCommand(SymbolGenerator.Generate());

        var response = await Client.PatchAsync(EndpointsConfigurations.CryptoEndpoints.UpdateInfo, request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnOk_WhenExistingSymbolProvided()
    {
        await Authenticate(UserRole.Admin);
        
        var entity = await Fixture.Add(new CryptoEntityBuilder().Build());
        CoinMarketCapClientFacade.MockValidResponse(Factory.MockServer, MockFixture, entity.Symbol);
        
        var request = new UpdateInfoCommand(entity.Symbol);
        var response = await Client.PatchAsync(EndpointsConfigurations.CryptoEndpoints.UpdateInfo, request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}