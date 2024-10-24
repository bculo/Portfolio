using System.Net;
using AutoFixture;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Infrastructure.Information.Models;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Helpers;
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
        //Arrange
        Client.WithRole(UserRole.Admin);
        var request = new AddNewCommand { Symbol = symbol };
    
        //Act
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task ShouldReturnStatusNoContent_WhenNewValidSymbolProvided()
    {
        Client.WithRole(UserRole.Admin);
        var request = new AddNewCommand { Symbol = SymbolGenerator.Generate() };

        await CoinMarketCapClientFacade.MockValidResponse(Factory.MockServer, MockFixture, request.Symbol);
        
        //Act
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenExistingSymbolProvided()
    {
        Client.WithRole(UserRole.Admin);
        var request = new AddNewCommand { Symbol = SymbolGenerator.Generate() };
        _ = await DataManager.AddInstance(request.Symbol);
        
        //Act
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

