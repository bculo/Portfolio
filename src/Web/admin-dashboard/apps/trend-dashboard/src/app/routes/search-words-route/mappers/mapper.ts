import { SearchWordResDto } from "../../../shared/services/open-api";
import { SearchWordFilterReqDto } from "../../../shared/services/open-api/model/search-word-filter-req-dto";
import { SearchWordFilterModel, SearchWordItem } from "../models/search-words.model";

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

export function mapToFilterViewModel(items: SearchWordResDto[]) {
    return items.map(x => ({
        id: x.id,
        contextTypeId: x.contextTypeId,
        contextTypeName: x.contextTypeName,
        created: x.created,
        isActive: x.isActive,
        searchEngineId: x.searchEngineId,
        searchEngineName: x.searchEngineName,
        searchWord: x.searchWord,
        imageUrl: x.imageUrl
    } as SearchWordItem))
}