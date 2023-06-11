using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Configurations.Persistence
{
    public static class MongoConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Article>(config =>
            {
                config.AutoMap();
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));
                config.MapMember(c => c.Type).SetSerializer(new EnumSerializer<ContextType>(MongoDB.Bson.BsonType.String));
            });

            BsonClassMap.RegisterClassMap<SyncStatus>(config =>
            {
                config.AutoMap();
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));
                config.UnmapMember(c => c.BadRequests);
            });

            BsonClassMap.RegisterClassMap<SearchWord>(config =>
            {
                config.AutoMap();
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));
                config.MapMember(c => c.Engine).SetSerializer(new EnumSerializer<SearchEngine>(MongoDB.Bson.BsonType.String));
            });

            BsonClassMap.RegisterClassMap<SyncStatusWord>(config =>
            {
                config.MapMember(c => c.Type).SetSerializer(new EnumSerializer<ContextType>(MongoDB.Bson.BsonType.String));
                config.MapMember(i => i.Word);
            });
        }
    }
}
