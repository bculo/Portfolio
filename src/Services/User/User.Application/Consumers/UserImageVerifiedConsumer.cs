using Events.Common.User;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using User.Application.Features;

namespace User.Application.Consumers;

public class UserImageVerifiedConsumer : IConsumer<UserImageVerified>
{
    private readonly IMediator _mediator;

    public UserImageVerifiedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }


    public async Task Consume(ConsumeContext<UserImageVerified> context)
    {
        var message = context.Message;
        if (message is { IsPerson: true, IsNsfw: false })
        {
            await _mediator.Send(new VerifyUserDto { UserName = context.Message.UserName });
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