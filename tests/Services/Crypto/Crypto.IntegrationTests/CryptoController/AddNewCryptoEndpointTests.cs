using System.Net;
using AutoFixture;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Infrastructure.Information.Models;
using Crypto.IntegrationTests.Common;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewCryptoEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
    {
        [Theory]
        [InlineData("BTC2")]
        [InlineData("---!-")]
        [InlineData("ETHIIIIIIIIIIIIIIIIIIIIIIIII")]
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
        
        
        [Theory]
        [InlineData("MATIC")]
        public async Task ShouldReturnStatusNoContent_WhenNewValidSymbolProvided(string symbol)
        {
            
            Client.WithRole(UserRole.Admin);
            var request = new AddNewCommand { Symbol = symbol };
            
            var infoResponse = MockFixture.Build<CoinMarketCapRootResponseDto>().Create();
            var firstItem = infoResponse.Data.Values!.FirstOrDefault();
            firstItem!.Insert(0, MockFixture.Build<CoinMarketCapInfoDto>()
                .With(x => x.Symbol, symbol)
                .With(x => x.Name, symbol)
                .Create());
            
            await Factory.MockServer.ReturnsWithJsonOk(infoResponse, withPath: "/info");
            
            //Act
            var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("MATIC")]
        [InlineData("BTC")]
        public async Task ShouldReturnBadRequest_WhenExistingSymbolProvided(string symbol)
        {
            Client.WithRole(UserRole.Admin);
            var request = new AddNewCommand { Symbol = symbol };
            _ = await DataManager.AddInstance(symbol);
            
            //Act
            var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
