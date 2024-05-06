export type GroupType = 'crypto' | 'stock';

export interface WebSocketService {
  connect(onConnectResult: (success: boolean) => void): void;
  joinGroup<T>(assetName: string, assetType: GroupType, onMessage: (message: T) => void): void;
  leaveGroup(assetName: string, assetType: GroupType): void;
}
