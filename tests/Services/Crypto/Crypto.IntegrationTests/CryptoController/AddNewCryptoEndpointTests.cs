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
            var request = new AddNewCommand { Symbol = Guid.NewGuid().ToString().Replace("-", "") };
            
            var infoResponse = MockFixture.Build<CoinMarketCapRootResponseDto>().Create();
            var firstItem = infoResponse.Data.Values!.FirstOrDefault();
            firstItem!.Insert(0, MockFixture.Build<CoinMarketCapInfoDto>()
                .With(x => x.Symbol, request.Symbol)
                .With(x => x.Name, request.Symbol)
                .Create());
            
            await Factory.MockServer.ReturnsWithJsonOk(infoResponse, withPath: "/info");
            
            //Act
            var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ShouldReturnBadRequest_WhenExistingSymbolProvided()
        {
            Client.WithRole(UserRole.Admin);
            var request = new AddNewCommand { Symbol = Guid.NewGuid().ToString().Replace("-", "") };
            _ = await DataManager.AddInstance(request.Symbol);
            
            //Act
            var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.Create, request.AsHttpContent());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
