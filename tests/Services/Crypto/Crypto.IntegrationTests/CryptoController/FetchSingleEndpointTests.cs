using System.Net;
using Crypto.API.Controllers;
using Crypto.Shared.Builders;
using Crypto.Shared.Utilities;
using FluentAssertions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class FetchSingleEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnNotFound_WhenNonExistentSymbolProvided()
    {
        await Authenticate(UserRole.Admin);
        
        var symbol = SymbolGenerator.Generate();
        var response = await Client.GetAsync(EndpointsConfigurations.CryptoEndpoints.BuildSingleUrl(symbol));
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldReturnOk_WhenExistentSymbolProvided()
    {
        await Authenticate(UserRole.Admin);
        
        var cryptoEntity = await Fixture.Add(new CryptoEntityBuilder().Build());
        await Fixture.AddPrice(new CryptoPriceEntityBuilder()
            .WithCryptoItemId(cryptoEntity.Id)
            .Build());
        
        var response = await Client.GetAsync(EndpointsConfigurations.CryptoEndpoints.BuildSingleUrl(cryptoEntity.Symbol));
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}