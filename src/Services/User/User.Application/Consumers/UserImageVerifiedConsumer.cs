using Events.Common.User;
using MassTransit;
using MediatR;
using User.Application.Features;

namespace User.Application.Consumers;

public class UserImageVerifiedConsumer(IMediator mediator) : IConsumer<UserImageVerified>
{
    public async Task Consume(ConsumeContext<UserImageVerified> context)
    {
        var message = context.Message;
        if (message is { IsPerson: true, IsNsfw: false })
        {
            await mediator.Send(new VerifyUserDto { UserName = context.Message.UserName });
        }
    }
}

public class UserImageVerifiedConsumerDefinition : ConsumerDefinition<UserImageVerifiedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<UserImageVerifiedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseRawJsonDeserializer();
    }
}