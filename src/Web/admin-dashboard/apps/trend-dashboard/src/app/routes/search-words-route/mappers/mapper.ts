import { SearchWordFilterReqDto } from "../../../shared/services/open-api/model/search-word-filter-req-dto";
import { SearchWordResDtoPageResponseDto } from "../../../shared/services/open-api/model/search-word-res-dto-page-response-dto";
import { SearchWordSyncDetailResDto } from "../../../shared/services/open-api/model/search-word-sync-detail-res-dto";
import { SearchWordFilterModel, SearchWordItem, SearchWordStats } from "../store/search-words.model";

export function mapToFilterReqDto(filter: SearchWordFilterModel) {
    return {
        active: filter.active,
        contextType: filter.contextType,
        page: filter.page,
        query: filter.query,
        searchEngine: filter.searchEngine,
        sort: filter.sort,
        take: filter.take
    } as SearchWordFilterReqDto
}

export function mapToFilterViewModel(items: SearchWordResDtoPageResponseDto) {
    return items.items?.map(x => ({
        id: x.id,
        contextTypeId: x.contextTypeId,
        contextTypeName: x.contextTypeName,
        created: x.created,
        isActive: x.isActive,
        searchEngineId: x.searchEngineId,
        searchEngineName: x.searchEngineName,
        searchWord: x.searchWord,
        imageUrl: x.imageUrl
    } as SearchWordItem)) ?? []
}


export function mapToSyncStatsViewModel(filter: SearchWordSyncDetailResDto) {
    return {
        count: filter.count,
        totalCount: filter.totalCount,
        wordId: filter.wordId
    } as SearchWordStats
}