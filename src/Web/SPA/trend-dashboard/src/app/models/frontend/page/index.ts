export interface PaginationState {
    take: number
    page: number,
    totalItems: number,
}

export interface PaginatedResult<T>{
    count: number
    items: T[]
}

export interface PageRequest {
    page: number,
    take: number
}