using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models.Services.Google
{
    public class GoogleSyncResult
    {
        /// <summary>
        /// Item 1 -> article type
        /// Item 2 -> searchWord
        /// Item 3 -> Http request status
        /// </summary>
        private readonly List<Tuple<ContextType, string, bool, GoogleSearchEngineResponseDto>> _requests;

        public int Total => _requests.Count;
        public int TotalSuccess => _requests.Count(i => i.Item3);

        private SyncStatus _syncStatus;
        public SyncStatus SyncStatus => _syncStatus;


        public GoogleSyncResult()
        {
            _requests = new List<Tuple<ContextType, string, bool, GoogleSearchEngineResponseDto>>();
        }

        public void AddResponse(ContextType type, string searchWord, bool requestStatus, GoogleSearchEngineResponseDto responseInstance)
        {
            _requests.Add(Tuple.Create(type, searchWord, requestStatus, responseInstance));
        }

        public void SetSyncInstance(SyncStatus syncStatusDto)
        {
            _syncStatus = syncStatusDto;
        }

        public List<GoogleSearchEngineResponseDto> GetInstances()
        {
            return _requests.Where(i => i.Item4 != null).Select(i => i.Item4!).ToList();
        }
    }
}
