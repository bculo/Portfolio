using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Application.Repositories.Lookups;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Configurations.Initialization
{
    public static class MongoConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<RootDocument>(config =>
            {
                config.AutoMap();
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));
            });

            BsonClassMap.RegisterClassMap<AuditableDocument>(config =>
            {
                config.AutoMap();
            });
            
            BsonClassMap.RegisterClassMap<Article>(config =>
            {
                config.AutoMap();
            });

            BsonClassMap.RegisterClassMap<SyncStatus>(config =>
            {
                config.AutoMap();
                config.UnmapMember(c => c.BadRequests);
            });

            BsonClassMap.RegisterClassMap<SearchWord>(config =>
            {
                config.AutoMap();
            });

            BsonClassMap.RegisterClassMap<ContextType>(config =>
            {
                config.MapProperty(x => x.Id);
                config.UnmapMember(x => x.Name);
                config.MapCreator(m => m.Id);
            });
            
            BsonClassMap.RegisterClassMap<SearchEngine>(config =>
            {
                config.MapProperty(x => x.Id);
                config.UnmapMember(x => x.Name);
                config.MapCreator(m => m.Id);
            });
            
            BsonClassMap.RegisterClassMap<SyncStatusWord>(config =>
            {
                config.AutoMap();
            });

            BsonClassMap.RegisterClassMap<ArticleSearchWordLookup>(config =>
            {
                config.AutoMap();
            });
            
            BsonClassMap.RegisterClassMap<ArticleDetailResQuery>(config =>
            {
                config.AutoMap();
            });
            
        }
    }
}
