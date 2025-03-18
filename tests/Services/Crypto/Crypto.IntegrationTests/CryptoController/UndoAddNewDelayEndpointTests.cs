using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay;
using Crypto.Infrastructure.Consumers.State;
using Crypto.Shared.Utilities;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class UndoAddNewDelayEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnNoContent_WhenCorrelationIdExists()
    {
        await Authenticate(UserRole.Admin);

        var delayRequest = new AddNewWithDelayCommand { Symbol = SymbolGenerator.Generate() };
        var delayResponse = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.CreateWithDelay,
            delayRequest.AsHttpContent());

        delayResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var correlationId = await delayResponse.ExtractContentFromResponse<Guid>();
        
        var request = new UndoNewWithDelayCommand { TemporaryId = correlationId };
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.UndoDelayCreate,
            request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var stateMachineHarness =
            MessageQueue.GetSagaStateMachineHarness<AddCryptoItemStateMachine, AddCryptoItemState>();
        var exists = await stateMachineHarness.Created.Any(x => x.CorrelationId == correlationId);
        exists.Should().BeTrue();
    }
    
    [Fact]
    public async Task ShouldReturnNoContent_CorrelationIdShouldntExist()
    {
        await Authenticate(UserRole.Admin);
        
        var request = new UndoNewWithDelayCommand { TemporaryId = Guid.NewGuid() };
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.UndoDelayCreate,
            request.AsHttpContent());
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var stateMachineHarness =
            MessageQueue.GetSagaStateMachineHarness<AddCryptoItemStateMachine, AddCryptoItemState>();
        var exists = await stateMachineHarness.Created.Any(x => x.CorrelationId == request.TemporaryId);
        exists.Should().BeFalse();
    }
}