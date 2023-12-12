using Events.Common.User;
using MassTransit;
using Newtonsoft.Json;

namespace User.Application.Consumers;

public class UserImageVerifiedConsumer : IConsumer<UserImageVerified>
{
    public Task Consume(ConsumeContext<UserImageVerified> context)
    {
        Console.WriteLine(JsonConvert.SerializeObject(context));
        return Task.CompletedTask;
    }
}