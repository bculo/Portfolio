export interface SyncStatus {
    id: string;
    started: Date,
    finished: Date,
    totalRequests: number;
    succeddedRequests: number;
    searchWords: SyncStatusWord[]
}

export interface SyncStatusWord {
    word: string;
    contextTypeName: number,
    contextTypeId: number
}

export interface AddSyncSetting {
    searchWord: string;
    searchEngine: number,
    contextType: number
}

export interface SyncExecuted {
    status: SyncStatus;
}
