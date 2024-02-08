using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Commands.Stock;
using Stock.Application.Queries.Stock;
using Stock.Core.Exceptions.Codes;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Stock.API.IntegrationTests.Stock;

public class ChangeStockStatusTests : StockControllerBaseTest
{
    public ChangeStockStatusTests(StockApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task ShouldReturnStatusOk_WhenStatusUpdatedSuccessfully()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        var symbol = "TSLA";
        var (_, id) = await Helper.CreateWithEncodedId(symbol);

        var command = new ChangeStockStatus(id);
        
        //Act
        var response = await Client.PutAsync($"/api/v1/stock/changestatus", command.AsHttpContent());

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task ShouldReturnStatusNotFound_WhenProblemOccursOnStatusUpdate()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        string id = Guid.NewGuid().ToString();

        var command = new ChangeStockStatus(id);
        
        //Act
        var response = await Client.PutAsync($"/api/v1/stock/changestatus", command.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Type.Should().Be(StockErrorCodes.STOCK_NOT_FOUND_BY_ID);
    }
}