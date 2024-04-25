import { ActiveEnumOptions, ContextTypeEnumOptions, SearchEngineEnumOptions, SortEnumOptions } from "../../../shared/enums/enums";

export interface SearchWordFilterModel { 
    page?: number;
    take?: number;
    active?: ActiveEnumOptions;
    contextType?: ContextTypeEnumOptions;
    searchEngine?: SearchEngineEnumOptions;
    query?: string | null;
    sort?: SortEnumOptions;
}

export interface SearchWordNewItem {
    searchEngine?: number;
    contextType?: number;
    searchWord: string | null
}

export interface SearchWordItem {
    id: string;
    isActive?: boolean;
    created?: Date;
    searchWord?: string | null;
    searchEngineName?: string | null;
    searchEngineId?: number;
    contextTypeName?: string | null;
    contextTypeId?: number;
    imageUrl: string | null
}

export interface SearchWordStats {
    wordId?: string | null;
    count?: number;
    totalCount?: number;
}