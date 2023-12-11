using Events.Common.User;
using MassTransit;

namespace User.Application.Consumers;

public class DeleteKeycloakUserConsumer : IConsumer<DeleteKeycloakUser>
{

    public Task Consume(ConsumeContext<DeleteKeycloakUser> context)
    {
        throw new NotImplementedException();
    }
}