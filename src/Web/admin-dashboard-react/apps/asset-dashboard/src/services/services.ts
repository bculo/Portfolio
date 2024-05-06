import * as signalR from '@microsoft/signalr';

import { injectable } from 'inversify';
import 'reflect-metadata';
import { GroupType, WebSocketService } from './interfaces';
import { getAccessToken } from '../utilities/token';
import { environment } from '../environments/environment';

const url = environment.websocket;
const authority = environment.oAuth2Config.authority;
const clientId = environment.oAuth2Config.client_id;

@injectable()
class SignalRConnector implements WebSocketService {
  private groups: string[] = [];
  private connection: signalR.HubConnection;
  private initialized = false;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: () => getAccessToken(authority, clientId),
      })
      .withAutomaticReconnect()
      .build();
  }

  public connect(onConnectResult: (success: boolean) => void): void {
    if (this.initialized) {
      return;
    }
    this.initialized = true;
    this.connection
      .start()
      .then((_) => onConnectResult(true))
      .catch((err) => {
        console.error(err);
        onConnectResult(false);
      });
  }

  public joinGroup<T>(
    assetName: string,
    assetType: GroupType,
    onMessage: (message: T) => void
  ): void {
    if (this.connection.state !== signalR.HubConnectionState.Connected) {
      return;
    }

    const groupName = this.formatGroupName(assetName, assetType);
    if (this.groups.includes(groupName)) {
      return;
    }

    this.groups.push(groupName);
    this.connection
      .invoke('JoinGroup', groupName)
      .catch((err) => console.log(err));

    this.connection.on(groupName, (data: T) => onMessage(data));
  }

  public leaveGroup(assetName: string, assetType: GroupType): void {
    if (this.connection.state !== signalR.HubConnectionState.Connected) {
      return;
    }

    const groupName = this.formatGroupName(assetName, assetType);
    this.groups = this.groups.filter((x) => x !== groupName);
    this.connection
      .invoke('LeaveGroup', groupName)
      .catch((err) => console.log(err));
  }

  private formatGroupName(assetName: string, assetType: GroupType): string {
    if (assetName.includes('.')) return assetName;
    return `${assetName.toLowerCase()}-${assetType}`;
  }
}

export { SignalRConnector };
