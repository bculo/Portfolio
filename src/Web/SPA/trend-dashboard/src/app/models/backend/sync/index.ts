export interface SyncStatus {
    id: string;
    started: Date,
    finished: Date,
    totalRequests: number;
    succeddedRequests: number;
}