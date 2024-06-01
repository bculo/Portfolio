using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Trend.Application.Repositories.Serializers;
using Trend.Domain.Entities;

namespace Trend.Application.Repositories.Initialization
{
    public static class MongoConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<RootDocument>(config =>
            {
                config.AutoMap();
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
            });
            
            BsonClassMap.RegisterClassMap<AuditableDocument>(config =>
            {
                config.AutoMap();
            });
            
            BsonSerializer.RegisterSerializer(new ContextTypeSerializer());
            BsonSerializer.RegisterSerializer(new SearchEngineSerializer());
            
            BsonClassMap.RegisterClassMap<Article>(config =>
            {
                config.AutoMap();
                config.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<SyncStatus>(config =>
            {
                config.AutoMap();
                config.SetIgnoreExtraElements(true);
                config.UnmapMember(c => c.BadRequests);
            });

            BsonClassMap.RegisterClassMap<SearchWord>(config =>
            {
                config.AutoMap();
                config.SetIgnoreExtraElements(true);
            });
            
            BsonClassMap.RegisterClassMap<SyncStatusWord>(config =>
            {
                config.AutoMap();
                config.SetIgnoreExtraElements(true);
            });
        }
    }
}
