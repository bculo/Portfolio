using Dtos.Common.v1.Trend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Models.Dtos.Google;
using Trend.Domain.Enums;

namespace Trend.Application.Models.Service.Google
{
    public class GoogleSyncResult
    {
        /// <summary>
        /// Item 1 -> article type
        /// Item 2 -> searchWord
        /// Item 3 -> Http request status
        /// </summary>
        private List<Tuple<ArticleType, string, bool, ArticleGroupDto?>> _requests;

        public GoogleSyncResult()
        {
            _requests = new List<Tuple<ArticleType, string, bool, ArticleGroupDto?>>();
        }


        public void AddResponse(ArticleType type, string searchWord, bool requestStatus, ArticleGroupDto? responseInstance)
        {
            _requests.Add(Tuple.Create(type, searchWord, requestStatus, responseInstance));
        }

        /// <summary>
        /// If all request succedded return true, otherwise false
        /// </summary>
        /// <returns></returns>
        private int CountResult()
        {
            return _requests.Count;
        }

        public List<ArticleGroupDto> GetInstances()
        {
            return _requests.Where(i => i.Item4 != null).Select(i => i.Item4!).ToList();
        }
    }
}
