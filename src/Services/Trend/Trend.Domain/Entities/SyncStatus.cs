using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class SyncStatus : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }
        public bool Success { get; set; }
    }
}
