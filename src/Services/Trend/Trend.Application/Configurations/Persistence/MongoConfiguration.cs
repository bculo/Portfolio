using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
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
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));

                config.MapMember(c => c.Type).SetSerializer(new EnumSerializer<ArticleType>(MongoDB.Bson.BsonType.String));
            });

            BsonClassMap.RegisterClassMap<SyncStatus>(config =>
            {
                config.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                config.IdMemberMap.SetSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));
            });
        }
    }
}
