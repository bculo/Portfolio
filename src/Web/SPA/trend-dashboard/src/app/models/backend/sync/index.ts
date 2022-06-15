export interface SyncStatus {
    id: string;
    started: Date,
    finished: Date,
    totalRequests: number;
    succeddedRequests: number;
}

export interface AddSyncSetting {
    searchWord: string;
    searchEngine: number,
    contextType: number
}