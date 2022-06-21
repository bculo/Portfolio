export interface InfinitePaginatedResult<T> {
    count: number
    page: number
    items: T[]
}