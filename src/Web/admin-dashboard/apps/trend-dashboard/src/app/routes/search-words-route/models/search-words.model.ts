export interface SearchWordFilterModel { 
    page?: number;
    take?: number;
    active?: number;
    contextType?: number;
    searchEngine?: number;
    query?: string | null;
    sort?: number;
}