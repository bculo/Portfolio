using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class Info : IDocument
    {
        public ObjectId Id { get; set; }
    }
}
