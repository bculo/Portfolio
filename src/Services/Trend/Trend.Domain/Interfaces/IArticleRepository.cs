using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Domain.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetArticles(DateTime from, DateTime to);
        Task<List<Article>> GetArticles(DateTime from, DateTime to, ArticleType type);
    }
}
