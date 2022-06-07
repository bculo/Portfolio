using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class Article : IDocument
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PageSource { get; set; }
        public string ArticleUrl { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public DateTime Created { get; set; }
        public ArticleType Type { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
