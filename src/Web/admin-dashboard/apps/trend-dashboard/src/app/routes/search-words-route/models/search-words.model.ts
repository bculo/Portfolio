export interface SearchWordFilterModel { 
    page?: number;
    take?: number;
    active?: number;
    contextType?: number;
    searchEngine?: number;
    query?: string | null;
    sort?: number;
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