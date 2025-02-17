namespace Events.Common;

public record IntegrationEvent(Type MessageType, object Message)
{
    public static IntegrationEvent From(object message) => new(message.GetType(), message);
}