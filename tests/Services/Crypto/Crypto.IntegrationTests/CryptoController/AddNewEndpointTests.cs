using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.IntegrationTests.Common;
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
        Client.WithRole(UserRole.Admin);
        var request = new AddNewCommand { Symbol = symbol };
        
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ShouldReturnStatusNoContent_WhenNewValidSymbolProvided()
    {
        Client.WithRole(UserRole.Admin);
        var request = new AddNewCommand { Symbol = SymbolGenerator.Generate() };

        await CoinMarketCapClientFacade.MockValidResponse(Factory.MockServer, MockFixture, request.Symbol);
        
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenExistingSymbolProvided()
    {
        Client.WithRole(UserRole.Admin);
        
        var entity = await DataManager.Add(new CryptoEntityBuilder().Build());
        
        var request = new AddNewCommand { Symbol = entity.Symbol };
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

