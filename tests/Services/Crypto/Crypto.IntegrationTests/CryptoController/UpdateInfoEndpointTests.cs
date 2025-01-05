using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Helpers;
using Crypto.Shared.Builders;
using Crypto.Shared.Utilities;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController;

public class UpdateInfoEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnNotFound_WhenNonExistentSymbolProvided()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        var request = new UpdateInfoCommand { Symbol = SymbolGenerator.Generate() };
    
        //Act
        var response = await Client.PutAsync(EndpointsConfigurations.CryptoEndpoints.UpdateInfo, request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnOk_WhenExistingSymbolProvided()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        
        var entity = await DataManager.Add(new CryptoEntityBuilder().Build());
        await CoinMarketCapClientFacade.MockValidResponse(Factory.MockServer, MockFixture, entity.Symbol);
        
        //Act
        var request = new UpdateInfoCommand { Symbol = entity.Symbol };
        var response = await Client.PutAsync(EndpointsConfigurations.CryptoEndpoints.UpdateInfo, request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}