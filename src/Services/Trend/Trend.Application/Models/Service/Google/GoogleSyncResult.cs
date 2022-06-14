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
        private List<Tuple<ContextType, string, bool, ArticleGroupDto?>> _requests;

        public int Total => _requests.Count;
        public int TotalSuccess => _requests.Count(i => i.Item3);

        public GoogleSyncResult()
        {
            _requests = new List<Tuple<ContextType, string, bool, ArticleGroupDto?>>();
        }

        public void AddResponse(ContextType type, string searchWord, bool requestStatus, ArticleGroupDto? responseInstance)
        {
            _requests.Add(Tuple.Create(type, searchWord, requestStatus, responseInstance));
        }


        public List<ArticleGroupDto> GetInstances()
        {
            return _requests.Where(i => i.Item4 != null).Select(i => i.Item4!).ToList();
        }
    }
}
