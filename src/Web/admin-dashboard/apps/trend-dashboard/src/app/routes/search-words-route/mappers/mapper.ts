import { SearchWordFilterReqDto } from "../../../shared/services/open-api/model/search-word-filter-req-dto";
import { SearchWordFilterModel } from "../models/search-words.model";

export function mapToDto(filter: SearchWordFilterModel) {
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