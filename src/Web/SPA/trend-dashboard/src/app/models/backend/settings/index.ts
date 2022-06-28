export interface Setting {
    id: string;
    searchWord: string;
    searchEngineName: string;
}

export interface CreateSetting {
    searchWord: string;
    searchEngine: 0;
    contextType: 0;
}