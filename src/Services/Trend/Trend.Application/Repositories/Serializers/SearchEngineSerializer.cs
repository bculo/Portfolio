using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Trend.Domain.Enums;

namespace Trend.Application.Repositories.Serializers;

public class SearchEngineSerializer : SerializerBase<SearchEngine>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SearchEngine value)
    {
        context.Writer.WriteInt32(value.Value);
    }

    public override SearchEngine Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return context.Reader.ReadInt32();
    }
}