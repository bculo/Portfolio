export interface WebSocketService {
    connect(onConnectResult: (success: boolean) => void): void
    joinGroup(assetName: string): void;
}