using Events.Common.User;
using MassTransit;
using Newtonsoft.Json;

namespace User.Application.Consumers;

public class UserImageVerifiedConsumer : IConsumer<UserImageVerified>
{
    public Task Consume(ConsumeContext<UserImageVerified> context)
    {
        Console.WriteLine(JsonConvert.SerializeObject(context.Message));
        return Task.CompletedTask;
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