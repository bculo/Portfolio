using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.Application.Modules.Crypto.Commands.UpdatePrice;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Helpers;
using Crypto.Shared.Utilities;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class UpdatePriceEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnNotFound_WhenNonExistentSymbolProvided()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        var request = new UpdatePriceCommand() { Symbol = SymbolGenerator.Generate() };
    
        //Act
        var response = await Client.PutAsync(EndpointsConfigurations.CryptoEndpoints.UpdatePrice, request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnOk_WhenExistingSymbolProvided()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        var request = new UpdatePriceCommand() { Symbol = SymbolGenerator.Generate() };
        _ = await DataManager.AddInstance(request.Symbol);
        
        var response = await Client.PutAsync(EndpointsConfigurations.CryptoEndpoints.UpdatePrice, request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}