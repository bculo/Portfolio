using FluentAssertions;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Moq;
using Tracker.Application.Enums;
using Tracker.Application.Infrastructure.Services;
using Tracker.Application.Options;
using Tracker.Core.Enums;

namespace Tracker.UnitTests.Infrastructure;

public class FinancialAssetClientFactoryTests
{
    private readonly IOptionsSnapshot<FinancalAssetClientFactoryOptions> _options;
    
    public FinancialAssetClientFactoryTests()
    {
        var options = new FinancalAssetClientFactoryOptions
        {
            CryptoService = ServiceCommunicationType.gRPC
        };
        
        var optionSnapshotMock = new Mock<IOptionsSnapshot<FinancalAssetClientFactoryOptions>>();
        optionSnapshotMock.Setup(x => x.Value).Returns(options);
        _options = optionSnapshotMock.Object;
    }

    [Fact]
    public void CreateClient_ShouldReturnInstance_WhenClientImplementationExists()
    {
        var providerMock = new Mock<IServiceProvider>();
        providerMock.Setup(x => x.GetService(typeof(CryptogRPCAssetClient)))
            .Returns(new CryptogRPCAssetClient(
                new Crypto.gRPC.Protos.v1.Crypto.CryptoClient(GrpcChannel.ForAddress("http://localhost"))));
        var _factory = new FinancialAssetClientFactory(providerMock.Object, _options);
        
        var result = _factory.CreateClient(FinancalAssetType.Crypto);

        result.Should().NotBeNull().And.BeOfType<CryptogRPCAssetClient>();
    }
    
}