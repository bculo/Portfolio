using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay;
using Crypto.Infrastructure.Consumers.State;
using Crypto.Shared.Utilities;
using Events.Common.Crypto;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class AddNewWithDelayEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnAccepted_WhenEndpointCalled()
    {
        await Authenticate(UserRole.Admin);

        var request = new AddNewWithDelayCommand { Symbol = SymbolGenerator.Generate() };
        var response = await Client.PostAsync(EndpointsConfigurations.CryptoEndpoints.CreateWithDelay,
            request.AsHttpContent());

        var correlationId = await response.ExtractContentFromResponse<Guid>();
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        
        var stateMachineHarness =
            MessageQueue.GetSagaStateMachineHarness<AddCryptoItemStateMachine, AddCryptoItemState>();

        var exists = await stateMachineHarness.Created.Any(x => x.CorrelationId == correlationId);
        exists.Should().BeTrue();
    }
}