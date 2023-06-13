using FluentAssertions;
using Stock.Worker.Services;

namespace Stock.Worker.UnitTests.Services
{
    public class WorkerUserServiceTests
    {
        [Fact]
        public void Identifier_ShouldReturnGuid_WhenCalled()
        {
            var service = new WorkerUserService();

            var identifier = service.Identifier;

            identifier.Should().NotBeEmpty();
        }
    }
}
