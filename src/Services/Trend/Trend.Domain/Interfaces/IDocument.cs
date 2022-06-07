using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Domain.Interfaces
{
    public interface IDocument
    {
        ObjectId Id { get; set; }
        public DateTime Created { get; set; }
    }
}
