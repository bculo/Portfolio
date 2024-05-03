export interface WebSocketService {
  connect(onConnectResult: (success: boolean) => void): void;
  joinGroup<T>(assetName: string, onMessage: (message: T) => void): void;
  leaveGroup(assetName: string): void;
}
