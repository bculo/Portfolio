using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Trend.Domain.Enums;

namespace Trend.Application.Repositories.Serializers;

public class ContextTypeSerializer : SerializerBase<ContextType>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ContextType value)
    {
        context.Writer.WriteInt32(value.Value);
    }

    public override ContextType Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return context.Reader.ReadInt32();
    }
}