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

public class AddNewEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Theory]
    [InlineData("")]
    [InlineData("----")]
    [InlineData("ETHII12!!!!!")]
    public async Task ShouldReturnBadRequest_WhenInvalidSymbolProvided(string symbol)
    {
        await Authenticate(UserRole.Admin);
        
        var request = new AddNewCommand(symbol, null);
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ShouldReturnStatusNoContent_WhenNewValidSymbolProvided()
    {
        await Authenticate(UserRole.Admin);
        var request = new AddNewCommand(SymbolGenerator.Generate(), null);

        CoinMarketCapClientFacade.MockValidResponse(Factory.MockServer, MockFixture, request.Symbol);
        
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenExistingSymbolProvided()
    {
        await Authenticate(UserRole.Admin);
        
        var entity = await Fixture.Add(new CryptoEntityBuilder().Build());
        
        var request = new AddNewCommand(entity.Symbol, null);
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

